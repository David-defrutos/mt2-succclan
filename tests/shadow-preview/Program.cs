// Documento generado el 2026-11-07-1205
// Documento generado el 2026-11-07-1154
using System.Collections;
using mt2_succclan.Plugin;
static class Program {
 static int checks;
 static void Check(bool ok,string msg){if(!ok)throw new Exception(msg);checks++;}
 static void Drain(IEnumerator r){try{while(r.MoveNext())if(r.Current is IEnumerator c)Drain(c);}finally{(r as IDisposable)?.Dispose();}}
 static List<float> Run(bool preview,int hp){
  var core=new Core(preview);var effect=new CardEffectEndlessShadowRebirth();var state=new CardEffectState();
  var source=new CardState(hp);var p=new CardEffectParams{playedCard=source,selfTarget=new CharacterState(source)};var result=new List<float>();
  for(int death=0;death<20&&effect.TestEffect(state,p,core);death++){
   Drain(effect.ApplyEffect(state,p,core,new Systems()));result.Add(core.Spawned!.UnitHP);
   p.playedCard=core.Spawned.Card;p.selfTarget=new CharacterState(p.playedCard);
   Check(EndlessShadowPreviewUpgrade.Scopes.Count==0,"scope after spawn");
  }
  Check(source.GetHealth()==hp,"source card unchanged");return result;
 }
 static void Main(){
  foreach(int hp in new[]{1,5,6,7,10,15,16,30,55}){
   var expected=new List<float>();for(int x=hp-5;x>0;x-=5)expected.Add(x);
   var real=Run(false,hp);var preview=Run(true,hp);
   Check(real.SequenceEqual(expected),"real chain "+hp);Check(preview.SequenceEqual(expected),"preview chain "+hp);
  }
  var data=new CardUpgradeData();var upgrade=new CardUpgradeState(data);var source=new CardState(15);var copy=new CardState(15);
  var unit=new CharacterState(copy){PreviewMode=true,SpawnedInPreviewMode=true};var scope=new EndlessShadowPreviewUpgrade.Scope(source,data);
  EndlessShadowPreviewUpgrade.Before(unit,upgrade,false);Check(copy.GetHealth()==15,"outside scope");
  EndlessShadowPreviewUpgrade.Scopes.Add(scope);
  try{
   EndlessShadowPreviewUpgrade.Before(unit,upgrade,true);Check(copy.GetHealth()==15,"fromSpawn excludes inherited upgrades");
   unit.SpawnedInPreviewMode=false;EndlessShadowPreviewUpgrade.Before(unit,upgrade,false);Check(copy.GetHealth()==15,"preexisting preview unit excluded");unit.SpawnedInPreviewMode=true;
   unit.PreviewMode=false;EndlessShadowPreviewUpgrade.Before(unit,upgrade,false);Check(copy.GetHealth()==15,"real unit excluded");unit.PreviewMode=true;
   unit.Card=source;EndlessShadowPreviewUpgrade.Before(unit,upgrade,false);Check(source.GetHealth()==15,"source identity excluded");unit.Card=copy;
   EndlessShadowPreviewUpgrade.Before(unit,new CardUpgradeState(new CardUpgradeData("other")),false);Check(copy.GetHealth()==15,"other upgrade excluded");
   EndlessShadowPreviewUpgrade.Before(unit,upgrade,false);EndlessShadowPreviewUpgrade.Before(unit,upgrade,false);Check(copy.GetHealth()==10,"bookkeeping exactly once, before unit penalty");Check(unit.UnitHP==15,"hook never applies unit HP");
  }finally{EndlessShadowPreviewUpgrade.Scopes.Remove(scope);}
  var core=new Core(true);var args=new CardEffectParams{playedCard=source,selfTarget=new CharacterState(source)};var effect=new CardEffectEndlessShadowRebirth();
  var r=effect.ApplyEffect(new CardEffectState(),args,core,new Systems());Check(r.MoveNext(),"scope entered");Check(EndlessShadowPreviewUpgrade.Scopes.Count==1,"scope active");((IDisposable)r).Dispose();Check(EndlessShadowPreviewUpgrade.Scopes.Count==0,"cancel cleanup");
  core.Throw=true;try{Drain(effect.ApplyEffect(new CardEffectState(),args,core,new Systems()));throw new Exception("expected failure");}catch(InvalidOperationException){}Check(EndlessShadowPreviewUpgrade.Scopes.Count==0,"exception cleanup");
  args.selfTarget!.IsDestroyed=true;Check(!effect.TestEffect(new CardEffectState(),args,core),"destroyed guard");
  Console.WriteLine($"PASS {checks} assertions: actual Endless Shadow source + preview hook; native spawn lifecycle modeled, not Unity.");
  Console.WriteLine("Real and preview: 15 -> 10 -> 5 -> stop; upgraded HP chains agree; cancellation and unrelated upgrades isolated.");
 }
}
public interface ICoreGameManagers{SaveManager GetSaveManager();}
public interface ISystemManagers{}
public sealed class SaveManager{public bool PreviewMode;}
public sealed class Core:ICoreGameManagers{readonly SaveManager save;public CharacterState? Spawned;public bool Throw;public Core(bool p){save=new(){PreviewMode=p};}public SaveManager GetSaveManager()=>save;}
public sealed class Systems:ISystemManagers{}
public sealed class PropDescriptions{}
public class CharacterState{public bool IsDestroyed;public bool IsDead=>true;public bool PreviewMode;public bool SpawnedInPreviewMode;public CardState Card;public float UnitHP;public CharacterState(CardState c){Card=c;UnitHP=c.GetHealth();}public CardState GetSpawnerCard()=>Card;public IEnumerator ApplyCardUpgrade(CardUpgradeState u,bool fromSpawn=false){yield break;}}
public class CardState{public float Health;public CardState(float hp){Health=hp;}public float GetHealth()=>Health;public Modifiers GetTemporaryCardStateModifiers()=>new(this);}
public class Modifiers{readonly CardState card;public Modifiers(CardState c){card=c;}public void AddUpgrade(CardUpgradeState u){card.Health-=5;}}
public class CharacterData{public float GetHealth()=>15;}
public class CardUpgradeData{readonly string id;public CardUpgradeData(string x="shadow"){id=x;}public string GetID()=>id;}
public class CardUpgradeState{readonly CardUpgradeData data;public CardUpgradeState(CardUpgradeData d){data=d;}public string GetCardUpgradeDataId()=>data.GetID();}
public class CardEffectState{readonly CardUpgradeData data=new();public bool GetParamBool()=>false;public CharacterData GetParamCharacterData()=>new();public CardUpgradeData GetParamCardUpgradeData()=>data;}
public class CardEffectParams{public CharacterState? selfTarget;public CardState? playedCard;}
public abstract class CardEffectBase{public virtual PropDescriptions CreateEditorInspectorDescriptions()=>new();public virtual bool TestEffect(CardEffectState e,CardEffectParams p,ICoreGameManagers c)=>true;public abstract IEnumerator ApplyEffect(CardEffectState e,CardEffectParams p,ICoreGameManagers c,ISystemManagers s);}
public sealed class CardEffectSpawnMonster:CardEffectBase{
 public override IEnumerator ApplyEffect(CardEffectState e,CardEffectParams p,ICoreGameManagers c,ISystemManagers s){
  var core=(Core)c;if(core.Throw)throw new InvalidOperationException();
  // Model the installed native order: copy card, construct unit, apply penalty,
  // record on card only !PreviewMode. Invoke actual Harmony prefix at that boundary.
  var card=new CardState(p.playedCard!.GetHealth());var unit=new CharacterState(card){PreviewMode=core.GetSaveManager().PreviewMode,SpawnedInPreviewMode=core.GetSaveManager().PreviewMode};
  var upgrade=new CardUpgradeState(e.GetParamCardUpgradeData());EndlessShadowPreviewUpgrade.Before(unit,upgrade,false);
  unit.UnitHP-=5;
  if(!core.GetSaveManager().PreviewMode)card.GetTemporaryCardStateModifiers().AddUpgrade(upgrade);
  core.Spawned=unit;yield break;
 }
}
namespace HarmonyLib{[AttributeUsage(AttributeTargets.Class)]public sealed class HarmonyPatch:Attribute{public HarmonyPatch(Type t,string n){}}[AttributeUsage(AttributeTargets.Method)]public sealed class HarmonyPrefix:Attribute{}}

// 2026-11-07-1154||codex-succclan-targetless||salidas\build\succclan-shadow-preview-2026-11-07-1154\Program.cs||Crear prueba del codigo real de preview, aislamiento y limpieza

// 2026-11-07-1205||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\shadow-preview\Program.cs||Consolidar suite local enlazando fuente real del repositorio externo
