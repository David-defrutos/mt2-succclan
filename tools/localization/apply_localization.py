import json,re,collections
from pathlib import Path
MOD=Path(__file__).resolve().parents[2]
WORK=Path(__file__).parent
LANGS=['spanish','french','german','portuguese','russian','chinese_traditional','japanese','korean']
SOURCES=json.loads((WORK/'english-unique.json').read_text(encoding='utf-8'))
NAMES={}
for line in (WORK/'names.tsv').read_text(encoding='utf-8').splitlines():
    if not line.strip():continue
    parts=line.split('|');assert len(parts)==9,parts
    NAMES[parts[0]]=dict(zip(LANGS,parts[1:]))
TEMPLATES={}
for line in (WORK/'templates.tsv').read_text(encoding='utf-8').splitlines():
    if not line.strip():continue
    parts=line.split('|');assert len(parts)==9,parts
    TEMPLATES[int(parts[0])]=dict(zip(LANGS,parts[1:]))
def remap(t,mapping):
    return re.sub(r'\{(\d+)\}',lambda m:mapping.get(int(m[1]),m[0]),t)
for lang in LANGS:
    for i,cost,multi in [(5,3,2),(6,4,4)]:
        TEMPLATES.setdefault(i,{})[lang]=TEMPLATES[4][lang].replace('$Burst 2','$Burst '+str(cost)).replace('{0} 1','{0} '+str(multi))
    prefix=remap(TEMPLATES[77][lang],{0:'1',1:'{0}'})+' <b>$Accursed</b>: '
    effects={46:remap(TEMPLATES[78][lang],{0:'1',1:'{1}',2:'1'}),
             48:remap(TEMPLATES[172][lang],{0:'20'}),
             50:remap(TEMPLATES[173][lang],{0:'{1}',1:'2'}),
             52:remap(TEMPLATES[175][lang],{0:'5',1:'{1}'}),
             54:remap(TEMPLATES[177][lang],{0:'{1}',1:''}).replace('{1} </b>','{1}</b>'),
             56:remap(TEMPLATES[179][lang],{0:'5'}),
             58:remap(TEMPLATES[181][lang],{0:'2',1:'{1}'})}
    for i,effect in effects.items():TEMPLATES.setdefault(i,{})[lang]=prefix+effect
    TEMPLATES.setdefault(174,{})[lang]=remap(TEMPLATES[77][lang],{0:'2',1:'{0}'})
TEMPLATES[176]=dict(zip(LANGS,['+{0}{1} y +{2}{3}.','+{0}{1} et +{2}{3}.','+{0}{1} und +{2}{3}.','+{0}{1} e +{2}{3}.','+{0}{1} и +{2}{3}.','+{0}{1} 與 +{2}{3}。','+{0}{1}と+{2}{3}。','+{0}{1} 및 +{2}{3}.']))

def glossary(s,lang):
    return re.sub(r'\$(Spark|Accursed|Burst|Ghosts|Ghost|Mutated|Elixirs|Psionic|Frantic|Shadow)',lambda m:NAMES[{'Spark':'Obsessing Spark','Burst':'Psionic Burst','Elixirs':'Mutant Elixirs','Shadow':'Endless Shadow'}.get(m[1],m[1])][lang],s)
def translated(i,lang):
    src=SOURCES[i]
    if i in TEMPLATES:
        tokens=re.findall(r'\[[^\]]+\]',src)
        return glossary(TEMPLATES[i][lang],lang).format(*tokens)
    if src in NAMES:return NAMES[src][lang]
    tier=re.match(r'^(.*) (II|III)$',src)
    if tier and tier[1] in NAMES:return NAMES[tier[1]][lang]+' '+tier[2]
    if src in ('Knightmare','Vrolikai','Oolioddroo'):return src
    if i in (68,74,176):return src
    if i in (98,163,167):
        return '<b>'+NAMES[re.sub('<[^>]+>','',src)][lang]+'</b>'
    if i in (161,165):
        return '<nobr>'+NAMES[src.split('>')[1].split(' ')[0]][lang]+' {0}</nobr>'
    raise ValueError((i,src,lang))
if __name__=='__main__':
    for i in range(len(SOURCES)):
        for lang in LANGS: translated(i,lang)
    ids={s:i for i,s in enumerate(SOURCES)}
    changed=[];fields=0
    def walk(x):
        global fields
        if isinstance(x,dict):
            if isinstance(x.get('english'),str):
                fields+=1
                for lang in LANGS:x[lang]=translated(ids[x['english']],lang)
            else:
                for v in x.values():walk(v)
        elif isinstance(x,list):
            for v in x:walk(v)
    for f in sorted((MOD/'json').rglob('*.json')):
        raw=f.read_bytes();obj=json.loads(raw.decode('utf-8-sig'));before=json.dumps(obj,ensure_ascii=False)
        walk(obj)
        if json.dumps(obj,ensure_ascii=False)!=before:
            backup=WORK/'backup'/f.relative_to(MOD);backup.parent.mkdir(parents=True,exist_ok=True)
            if not backup.exists():backup.write_bytes(raw)
            newline='\r\n' if b'\r\n' in raw else '\n'
            data=(json.dumps(obj,ensure_ascii=False,indent=4)+'\n').replace('\n',newline).encode('utf-8')
            if raw.startswith(b'\xef\xbb\xbf'):data=b'\xef\xbb\xbf'+data
            f.write_bytes(data);changed.append(f.relative_to(MOD).as_posix())
    print(json.dumps({'fields':fields,'files':len(changed),'languages':LANGS},ensure_ascii=True))
