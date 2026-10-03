import json,re,collections,subprocess,sys
from pathlib import Path
MOD=Path(__file__).resolve().parents[2]
WORK=Path(__file__).parent
LANGS=['english','chinese','spanish','french','german','portuguese','russian','chinese_traditional','japanese','korean']
issues=[];counts=collections.Counter();fields=0
TOKENS=r'\[[^\]]+\]|\{\d+\}'
def stripped(x):
    if isinstance(x,dict):
        if 'english' in x:return {'english':x['english']}
        return {k:stripped(v) for k,v in x.items()}
    if isinstance(x,list):return [stripped(v) for v in x]
    return x

def numbers(s):
    s=re.sub(TOKENS,'',s)
    s=re.sub(r'<[^>]+>','',s)
    # Japanese counters make English singular targets/"for each" explicit; 2倍 translates "double".
    s=s.replace('1体','体').replace('1枚につき','枚につき').replace('2倍','倍')
    # English "an Obsessing Spark" explicitly means one card.
    s=s.replace('an Obsessing Spark','1 Obsessing Spark')
    return collections.Counter(re.findall(r'[0-9]+',s))

def walk(obj,before,path):
    global fields
    if isinstance(obj,dict):
        if isinstance(obj.get('english'),str):
            fields+=1
            assert obj['english']==before['english'],path
            assert obj['chinese']==before['chinese'],path
            for lang in LANGS:
                text=obj.get(lang)
                if not isinstance(text,str) or not text.strip():issues.append([path,lang,'missing']);continue
                counts[lang]+=1
                stack=[]
                for tag in re.findall(r'</?(?:b|nobr|i)>',text):
                    if tag.startswith('</'):
                        if not stack or stack.pop()!=tag[2:-1]:issues.append([path,lang,'unbalanced markup'])
                    else:stack.append(tag[1:-1])
                if stack:issues.append([path,lang,'unclosed markup'])
                if lang not in ('english','chinese'):
                    for label,pattern in [('tokens',TOKENS),('markup',r'</?(?:b|nobr|i)>')]:
                        if collections.Counter(re.findall(pattern,text))!=collections.Counter(re.findall(pattern,obj['english'])):issues.append([path,lang,label])
                    if numbers(text)!=numbers(obj['english']):issues.append([path,lang,'quantities',dict(numbers(obj['english'])),dict(numbers(text))])
                if '$' in text:issues.append([path,lang,'unresolved glossary'])
        else:
            for key,value in obj.items():walk(value,before[key],path+'/'+key)
    elif isinstance(obj,list):
        for i,value in enumerate(obj):walk(value,before[i],path+'/'+str(i))
files=0
for f in sorted((MOD/'json').rglob('*.json')):
    obj=json.loads(f.read_text(encoding='utf-8-sig'))
    backup=WORK/'backup'/f.relative_to(MOD)
    baseline=sys.argv[1] if len(sys.argv)>1 else '32c0c3e'
    before=json.loads(backup.read_text(encoding='utf-8-sig') if backup.exists() else subprocess.check_output(['git','show',baseline+':'+f.relative_to(MOD).as_posix()],cwd=MOD).decode('utf-8-sig'))
    assert stripped(obj)==stripped(before),f
    walk(obj,before,f.relative_to(MOD).as_posix());files+=1
report={'files':files,'fields':fields,'coverage':dict(counts),'issues':issues,'gameplay_unchanged':True,'english_and_simplified_chinese_unchanged':True,'native_speaker_review':False,'all_languages_in_game_verified':False}
(WORK/'validation.json').write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
print(json.dumps(report,ensure_ascii=True))
assert not issues
