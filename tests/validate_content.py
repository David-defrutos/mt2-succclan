# Documento generado el 2026-11-07-1205
# Documento generado el 2026-10-27-1551

from pathlib import Path
import json,re,hashlib
mod=Path(__file__).resolve().parents[1]
checks=0
def check(ok,m):
 global checks
 assert ok,m
 checks+=1
def load(rel):return json.loads((mod/rel).read_text(encoding='utf-8-sig'))
paths=list((mod/'json').rglob('*.json'));registered=re.findall(r'"(json/[^"\r\n]+\.json)"',(mod/'src/Plugin.cs').read_text(encoding='utf-8-sig'))
check(set(registered)=={p.relative_to(mod).as_posix() for p in paths},'all JSON registered')
sources='\n'.join(p.read_text(encoding='utf-8-sig') for p in (mod/'src/code').glob('*.cs'))
classes=set(re.findall(r'\bclass\s+(\w+)',sources))
for p in paths:
 d=json.loads(p.read_text(encoding='utf-8-sig'));checks+=1
 for category,items in d.items():
  if isinstance(items,list):
   ids=[x['id'] for x in items if isinstance(x,dict) and isinstance(x.get('id'),str)];check(len(ids)==len(set(ids)),str(p)+' duplicate IDs')
   if category in ('effects','traits','relic_effects','room_modifiers','status_effects'):
    for item in items:
     name=item.get('name',item.get('class_name',''))
     if isinstance(name,str) and name.startswith('@'):check(name[1:] in classes,'custom class missing '+name)
 for sprite in d.get('sprites',[]):check((mod/sprite['path']).is_file(),'missing sprite')
 check(not re.search(r'"(?:applied_vfx|triggered_vfx)"\s*:\s*"@SuccFXObsessionPulse"',p.read_text(encoding='utf-8-sig')),'active Moonrise')
 for e in d.get('effects',[]):
  if e.get('name')=='CardEffectAddStatusEffect':check(len(e.get('param_status_effects',[]))<=1,'unexpected random status list '+e['id'])
for rel in ['json/spells/card_MindDomination.json','json/spells/card_ProfaneAscending.json']:
 d=load(rel);byid={e['id']:e for e in d['effects']};card=d['cards'][0];gate=byid[card['effects'][0][1:]];bump=byid[card['effects'][1][1:]]
 check(gate['name']=='CardEffectNULL' and gate['target_mode']=='drop_target_character','native NULL gate')
 check(gate['target_ignore_bosses'] and gate['should_fail_to_cast_if_test_fails'] and gate['should_cancel_subsequent_effects_if_test_fails'],'whole card rejects invalid target')
 check(bump['name']=='CardEffectBump' and bump['target_mode']=='last_targeted_characters','movement retains validated target')
 for desc in card['descriptions'].values():check('[effect2.status0.power]' in desc and '[effect3.power]' in desc,'shifted dynamic text indices')
for rel,eid in [('json/spells/card_PainAndPleasure.json','PainAndPleasureDiscard'),('json/units/unit_ArroganceGhost.json','ArroganceGhostSpread')]:
 e=next(e for e in load(rel)['effects'] if e['id']==eid)
 check(e['name']=={'id':'@CardEffectRandomDiscardFromCardPile','mod_reference':'Conductor'},'library discard')
 check(e['target_mode']=='hand' and e['target_card_type']=='blight' and e['param_card_pool']=='@ObsessingShardPool' and e['param_int']==2,'exact Spark discard contract')
pain=load('json/spells/card_PainAndPleasure.json');check(pain['effects'][1]['param_int']==3,'Pain still draws three')
mutant=load('json/relics/relic_MutantElixirs.json');byid={e['id']:e for e in mutant['effects']};check(next(e for e in mutant['relic_effects'] if e['id']=='MutantElixirsGrant')['param_effects']==['@MutantGiveMark','@MutantGiveMultistrike'],'mutation order')
check(byid['MutantGiveMark']['target_mode']=='random_in_room' and byid['MutantGiveMark']['param_status_effects']==[{'status':'@mutated','count':1}],'mark chosen unit')
check(byid['MutantGiveMultistrike']['target_mode']=='front_with_status' and byid['MutantGiveMultistrike']['param_str']=='mt2_succclan.Plugin_mutated' and byid['MutantGiveMultistrike']['param_status_effects']==[{'status':'multistrike','count':1}],'buff same marked unit')
check(all(byid[x]['param_bool'] for x in ['MutantGiveMark','MutantGiveMultistrike']),'empty rooms rejected')
check(all(byid[x]['param_str']=='mt2_succclan.Plugin_mutated' for x in ['MutantTakeMultistrike','MutantClearMark']),'marker case preserved')
for e in load('json/champions/champion_KnightMare.json')['character_triggers']:
 if e['id'].startswith('Endless'):
  for text in e['descriptions'].values():check('[effect0.power]' in text,'dynamic rebirth cost')
inception=load('json/spells/card_Inception.json')['cards'][0];check(inception['cost']==1 and inception['effects']==['@InceptionPsionic','@InceptionAttack','@InceptionHealth'],'Inception unchanged')
check(load('json/status_effects/psionic.json')['status_effects'][0]['trigger_stage']=='none','no Psionic redesign')
check('animations' not in load('json/units/unit_IncubusButcher.json')['game_objects'][1]['extensions']['character_art'],'Butcher static')
profane=load('json/spells/card_ProfaneAscending.json');byid={e['id']:e for e in profane['effects']}
check(all(byid[x]['target_team']=='heroes' for x in ['ProfaneAscendingValidTarget','PABump','PAFrantic','PAHeal']),'Profane is enemy-only throughout validation and resolution')
check(load('json/spells/card_MindDomination.json')['effects'][0]['target_team']=='both','Mind Domination still accepts both teams')
frantic=load('json/status_effects/frantic.json')['status_effects'][0]
check(frantic['card_tooltips']==frantic['character_tooltips'] and len(frantic['card_tooltips'])==10,'Frantic card and unit tooltips agree')
check('possibly itself' in frantic['card_tooltips']['english'] and 'si ataca' in frantic['card_tooltips']['spanish'],'self-target and retained charges explained')
blade=load('json/equipment/equip_WhisperingBlade.json');check(blade['character_triggers'][0]['trigger']=={'id':'@Accursed','mod_reference':'Conductor'},'Whispering Blade Accursed')
check(blade['effects'][1]['target_mode']=='back_in_room' and blade['effects'][1]['target_team']=='heroes' and blade['effects'][1]['param_status_effects']==[{'status':'debuff','count':2}],'Whispering Blade native Sap2 rear enemy')
manifest=load('manifest.json');check('Conductor-Conductor-0.5.17' in manifest['dependencies'] and 'MT2-Trainworks_Reloaded-0.7.35' in manifest['dependencies'],'runtime supports pool filter')
for p in (mod/'src/code').glob('*.cs'):
 b=p.read_bytes()
 if b'2026-10-27' in b:check(not b.startswith(b'\xef\xbb\xbf') and b'\r\n' not in b,'C# LF no BOM')

stone=next(x for x in load('json/plugin.json')['relics'] if x['id']=='CurseguardStone')
check('pools' not in stone,'no ignored top-level enhancer pools')
check(set(stone['extensions'][0]['enhancer']['pools'])=={'UnitUpgradePoolCommon','DraftUpgradePool','MalickaDraftUpgradePool'},'shop pools at path actually consumed by Trainworks')
check(all('[effect0.upgrade.charactertrigger0.effect0.status0.power]' in x for x in stone['descriptions'].values()),'dynamic Armor in enhancer text')
check('CardsThatResolveSimultaneouslyOnUnplayed' in load('json/spells/card_ObsessingShard.json')['cards'][0]['pools'],'native simultaneous Spark pool')
check(len(load('json/relics/relic_FleshRing.json')['relics'][0]['relic_activated_texts'])==10,'Flesh Ring activation localized')
check(all('[effect0.status0.power]' in x and '[effect2.status0.power]' in x for x in load('json/spells/card_VitalityExtraction.json')['cards'][0]['descriptions'].values()),'Vitality quantities dynamic')
plague=next(x for x in load('json/plugin.json')['traits'] if x['id']=='PlagueBoostScaling')
check(plague['param_tracked_value']=={'id':'@BlightsAndScourgesInDeck','mod_reference':'Conductor'},'Plague Boost counts both types natively')
shadow=load('json/champions/champion_ShadowLady.json')
check(next(x for x in shadow['room_modifiers'] if x['id']=='ShadowLadyPlagueEmber')['name']=='RoomStateEnergyModifier','native energy and preview')
check(sum('@ShadowLadyPlagueEmber' in x.get('room_modifier_upgrades',[]) for x in shadow['upgrades'])==3,'all Plague tiers use native ember modifier')
check(all(len(x['effects'])==1 for x in shadow['character_triggers'] if x['id'] in ['PlagueBasicPreCombat','PlaguePrePreCombat','PlagueProPreCombat']),'no double ember grant')

plugin=load('json/plugin.json')
slab=next(x for x in plugin['relics'] if x['id']=='CurseguardSlab')
check(slab['extensions'][0]['enhancer']['pools']==['SoulSavior_UnitUpgradePoolCommon'],'Slab exclusive to empowered Soul Savior pool')
check(slab['relic_effects']==['@CurseguardSlabApply'],'Slab applies its own upgrade')
check(next(x for x in plugin['effects'] if x['id']=='CurseguardSlabArmor')['param_status_effects']==[{'status':'armor','count':10}],'Slab Armor10')
check(next(x for x in plugin['effects'] if x['id']=='CurseguardStoneArmor')['param_status_effects']==[{'status':'armor','count':5}],'Stone retains Armor5')
check(next(x for x in plugin['upgrades'] if x['id']=='CurseguardSlabUpgrade')['trigger_upgrades']==['@CurseguardSlabAccursed'],'Slab upgrade links to native Accursed trigger')
for x in shadow['upgrades']:
 if x['id'].startswith('upg_BloodThirsty'):check(x['bonus_size']==-1,'all Bloodthirsty tiers reduce size once')
 elif x['id'].startswith('upg_'):check(x.get('bonus_size',0)==0,'other paths keep size')
check(shadow['characters'][0]['size']==3,'base size unchanged; native upgrade sets Bloodthirsty to two')
# Native threshold flag prevents Crown from changing the displayed payment.
thresholds=[e for e in load('json/champions/champion_KnightMare.json')['effects'] if isinstance(e.get('name'),dict) and e['name'].get('id')=='@CardEffectRequireStatusThreshold']
check(len(thresholds)==6,'six native Burst thresholds')
check(all(e.get('disallow_status_effect_stack_modifiers') is True for e in thresholds),'Burst tooltip shows raw payment with Crown')
activation=load('json/relics/relic_DesireCrystal.json')['relics'][0]['relic_activated_texts']
check(len(activation)==10 and all(v.strip() and 'NULL' not in v and '[' not in v for v in activation.values()),'Desire Crystal localized literal activation messages')
check(next(e for e in load('json/essences.json')['upgrades'] if e['id']=='EssenceSuccbusTorturer')['titles']['english']=='Succubus Torturer','essence display corrected; stable internal ID')
from PIL import Image
for name in ('CurseguardStone','CurseguardSlab'):
 relic=next(e for e in plugin['relics'] if e['id']==name)
 check(relic['icon']=='@'+name+'Icon' and relic['icon_small']=='@'+name+'IconSmall','separate enhancer icons')
 for suffix,size in (('Icon',256),('IconSmall',96)):
  sprite=next(e for e in plugin['sprites'] if e['id']==name+suffix)
  with Image.open(mod/sprite['path']) as img:
   check(img.size==(size,size) and img.mode=='RGBA' and img.getextrema()[3][0]==0,'correct icon dimensions and transparent background')
essences=load('json/essences.json')
for uid in ('EssenceLustGhost','EssenceAbyssPrincess','EssenceChaosCreation'):
 e=next(x for x in essences['upgrades'] if x['id']==uid)
 check(len(e['descriptions'])==10,'localized essence restrictions')
 check(all('[charactertrigger0.effect' in t for t in e['descriptions'].values()),'essence stack counts from real trigger')
check(all('[charactertrigger0.effect1.status0.power]' in t and '[charactertrigger0.effect2.status0.power]' in t for t in next(x for x in essences['upgrades'] if x['id']=='EssenceAbyssPrincess')['descriptions'].values()),'Princess dynamic Rage and Frantic')
check('Cannot target bosses.' in next(x for x in essences['character_triggers'] if x['id']=='EssenceChaosOnHit')['descriptions']['english'],'Chaos unit tooltip boss restriction')
print(json.dumps({'json_files':len(paths),'content_assertions':checks,'result':'PASS'},ensure_ascii=False))

# 2026-10-27-1551||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\ValidateContent.py||Crear contratos de contenido para equivalencias nativas, validación, Mutant Elixirs y decisiones conservadas

# 2026-10-27-2302||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\ValidateContent.py||Añadir contrato H1 enemy-only y conservar Mind both; textos Frantic carta/unidad y Sap2 trasero
# 2026-11-05-1730||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\ValidateContent.py||Añadir contratos de pools en ruta real, tokens, activacion Flesh Ring, Plague Boost y energia nativa Shadow Lady
# 2026-11-05-1806||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\ValidateContent.py||Validar Slab exclusiva Armor10, Stone5, cadenas de referencias y tamaño por senda

# 2026-11-07-1144||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\ValidateContent.py||Validar umbrales Burst nativos, mensaje Desire Crystal, nombre esencia e iconos RGBA propios

# 2026-11-07-1155||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\ValidateContent.py||Validar cantidades dinamicas de tres esencias y restriccion del tooltip Chaos

# 2026-11-07-1205||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\validate_content.py||Consolidar contratos de contenido sobre el repositorio externo
