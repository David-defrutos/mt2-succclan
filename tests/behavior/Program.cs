// Documento generado el 2026-11-07-1205
// Documento generado el 2026-10-27-1459

using System.Collections;
using System.Reflection;
using mt2_succclan.Plugin;
class Program {
 static int checks;
 static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
 static void Drain(IEnumerator e){try{while(e.MoveNext())if(e.Current is IEnumerator nested)Drain(nested);}finally{(e as IDisposable)?.Dispose();}}
 static IEnumerator Do(Action a){a();yield break;}
 static object? Call(Type t,string method,params object?[] a)=>t.GetMethod(method,BindingFlags.Static|BindingFlags.NonPublic)!.Invoke(null,a);
 static CharacterState Knight(int tier,int psionic,bool preview=false){var u=new CharacterState{PreviewMode=preview};u.Status[CardEffectKnightmareRebirth.Psionic]=psionic;u.Triggers.Add(new CharacterTriggerState{Effects=new(){new CardEffectState{Effect=new CardEffectKnightmareRebirth(),Upgrade=new CardUpgradeData{Tier=tier}}}});return u;}
 static IEnumerator NativeRevive(CharacterState u,Action? beforeHarvest=null){u.HP=1;u.RemoveStatusEffect("undying",1);yield return null;beforeHarvest?.Invoke();}
 static IEnumerator NativeCheck(CharacterState u,Action? beforeHarvest=null){if(u.GetStatusEffectStacks("undying")>0)yield return KnightmareNativeRebirth.Revive(NativeRevive(u,beforeHarvest),u);}
 static void Main(){
 foreach(int tier in new[]{1,2,3})foreach(bool preview in new[]{false,true}){
  var u=Knight(tier,20,preview);var card=u.Card;
  for(int i=0;i<4;i++){
   u.HP=0;int cost=2+i,before=u.GetStatusEffectStacks(CardEffectKnightmareRebirth.Psionic);
   Check(CardEffectKnightmareRebirth.GetRebirthCost(u.Triggers[0].Effects[0],u)==cost,"increasing cost");
   Drain(KnightmareNativeRebirth.Check(NativeCheck(u,()=>Check(u.HP==u.MaxHP,"full health BEFORE Harvest")),u,false));
   Check(ReferenceEquals(card,u.Card),"same card preserved");Check(u.HP==10+(i+1)*tier,"full upgraded health");Check(u.Attack==3+(i+1)*tier,"tier attack growth");Check(u.GetStatusEffectStacks("multistrike")==i+1,"one multistrike per rebirth");Check(u.GetStatusEffectStacks(CardEffectKnightmareRebirth.Psionic)==before-cost,"exact Psionic cost");Check(u.GetStatusEffectStacks("undying")==0,"borrowed undying consumed");
  }
  Check(u.Card.Mods.Upgrades.Count==(preview?0:4),"preview never mutates real card modifiers");
 }
 foreach(string blocked in new[]{"silenced","muted","insufficient","ignore","disabled"}){
  var u=Knight(1,blocked=="insufficient"?1:2);if(blocked is "silenced" or "muted")u.Status[blocked]=1;if(blocked=="disabled")u.Point.Room.Enabled=false;
  Drain(KnightmareNativeRebirth.Check(NativeCheck(u),u,blocked=="ignore"));Check(u.HP==0,"blocked rebirth "+blocked);Check(u.Upgrades.Count==0,"no growth on blocked rebirth");
 }
 {var u=Knight(1,5);u.Status["undying"]=2;for(int i=0;i<2;i++){u.HP=0;Drain(KnightmareNativeRebirth.Check(NativeCheck(u),u,false));Check(u.HP==1,"native Undying health retained");Check(u.GetStatusEffectStacks(CardEffectKnightmareRebirth.Psionic)==5,"native Undying keeps Psionic");Check(u.Upgrades.Count==0,"native Undying no Endless growth");}u.HP=0;Drain(KnightmareNativeRebirth.Check(NativeCheck(u),u,false));Check(u.HP==11,"Psionic rebirth after Undying exhausted");}
 {var u=Knight(1,2);u.HP=4;Drain(KnightmareNativeRebirth.Check(NativeCheck(u),u,false));Check(u.HP==4&&u.Upgrades.Count==0,"live unit unaffected");u.Destroyed=true;Drain(KnightmareNativeRebirth.Check(Do(()=>throw new Exception("must not run")),u,false));Check(true,"destroyed source guarded");}
 {var u=Knight(1,2);var p=new CardEffectParams{selfTarget=u,dyingCharacter=u};var effect=new CardEffectKnightmareHarvest();Drain(effect.ApplyEffect(new CardEffectState(),p,null!,null!));Check(!u.Status.ContainsKey("harvest"),"own death excluded");p.dyingCharacter=new CharacterState();Drain(effect.ApplyEffect(new CardEffectState(),p,null!,null!));Check(u.Status["harvest"]==1,"other death retained");}
 {var u=Knight(1,10);Drain(KnightmareNativeRebirth.Check(NativeCheck(u),u,false));var t=new TranslationTokenCardText{elements=new(){new TranslationTokenCardTextElement{ElementType=TranslationTokenCardTextElement.TranlationTokenCardTextElementType.Effect,SubType=TranslationTokenCardTextElement.ParamSubType.Power}}};object?[] args={u.Triggers[0],u,t,(int?)2};Call(typeof(KnightmareRebirthCostText),"After",args);Check((int?)args[3]==3,"live tooltip cost");}
 var frantic=new StatusEffectFranticState();var unit=new CharacterState{HP=10};unit.Status["mt2_succclan.Plugin_frantic"]=2;var input=new StatusEffectState.InputTriggerParams{associatedCharacter=unit};var output=new StatusEffectState.OutputTriggerParams();
 Check(!FranticNativeTurn.IsActive(unit),"no leaked context initially");
 Drain(FranticNativeTurn.Run(Do(()=>{Check(frantic.TestTrigger(input,output,null!),"Frantic valid");Drain(frantic.Fire(input,output));Check(output.canAttackOrHeal,"native attack kept enabled");Check(unit.GetStatusEffectStacks("mt2_succclan.Plugin_frantic")==2,"Spark stage retains charge before native attack checks");int callbacks=0;object?[] callbackArgs={unit,(Action<int>)(_=>callbacks++)};Call(typeof(FranticNativeTurnPatch),"Before",callbackArgs);var callback=(Action<int>)callbackArgs[1]!;callback(0);Check(unit.GetStatusEffectStacks("mt2_succclan.Plugin_frantic")==1,"first actual attack consumes one charge");callback(1);callback(2);Check(callbacks==3,"existing per-attack callback preserved for every Multistrike hit");Check(unit.GetStatusEffectStacks("mt2_succclan.Plugin_frantic")==1,"Multistrike consumes only once per combat turn");Check(FranticNativeTurn.IsActive(unit),"scope active during attacks");}),unit));Check(!FranticNativeTurn.IsActive(unit),"scope cleared after turn");
 foreach(bool companion in new[]{false,true}){unit.Boss=!companion;unit.Companion=companion;Check(!frantic.TestTrigger(input,output,null!),"boss immunity");}unit.Boss=unit.Companion=false;output.canAttackOrHeal=false;Check(!frantic.TestTrigger(input,output,null!),"Dazed does not consume Frantic");output.canAttackOrHeal=true;
 {var front=new CharacterState{HP=4};unit.Point.Room.Units=new(){front,unit};Drain(FranticNativeTurn.Run(Do(()=>{Drain(frantic.Fire(input,output));object?[] a={unit,new CharacterManager(),null};Call(typeof(FranticNativeTargetsPatch),"Before",a);Check(ReferenceEquals(a[1],unit.Manager),"native target manager own team");Check(ReferenceEquals(((List<CharacterState>)a[2]!)[0],front),"front ally targeted");front.Destroyed=true;Call(typeof(FranticNativeTargetsPatch),"Before",a);Check(ReferenceEquals(((List<CharacterState>)a[2]!)[0],unit),"retarget after front ally destroyed");unit.Destroyed=true;Check(frantic.GetEffectMagnitude()==0,"no destroyed magnitude access");}),unit));unit.Destroyed=false;Check(!FranticNativeTurn.IsActive(unit),"self-kill scope cleaned");}
 {var e=FranticNativeTurn.Run(Do(()=>throw new InvalidOperationException()),unit);try{Drain(e);}catch(InvalidOperationException){}Check(!FranticNativeTurn.IsActive(unit),"exception scope cleaned");}
 // Simulate the native boundary: the per-attack callback is only invoked
 // after CheckAttackConditions and successful target collection.
 foreach(string blocked in new[]{"Sap0","noAttacks","noTargets","actionKilled"}){
  var u=new CharacterState{HP=8,Attack=blocked=="Sap0"?0:3};const string id="mt2_succclan.Plugin_frantic";u.Status[id]=2;
  var i=new StatusEffectState.InputTriggerParams{associatedCharacter=u};var o=new StatusEffectState.OutputTriggerParams();
  object?[] args={u,null};Call(typeof(FranticNativeTurnPatch),"Before",args);var callback=(Action<int>)args[1]!;
  Drain(FranticNativeTurn.Run(Do(()=>{Drain(frantic.Fire(i,o));Check(FranticNativeTurn.IsActive(u),"pending Frantic redirect: "+blocked);if(blocked=="actionKilled")u.HP=0;/* native would not call callback */Check(u.GetStatusEffectStacks(id)==2,"no attack preserves charge: "+blocked);}),u));
  Check(!FranticNativeTurn.IsActive(u),"pending scope cleaned: "+blocked);
  u.HP=8;u.Attack=3;Drain(FranticNativeTurn.Run(Do(()=>{Drain(frantic.Fire(i,o));callback(0);Check(u.GetStatusEffectStacks(id)==1,"retained charge usable next turn: "+blocked);}),u));
 }
 {var u=new CharacterState{HP=8,Attack=0};const string id="mt2_succclan.Plugin_frantic";u.Status[id]=2;object?[] args={u,null};Call(typeof(FranticNativeTurnPatch),"Before",args);var callback=(Action<int>)args[1]!;
  Drain(FranticNativeTurn.Run(Do(()=>{Drain(frantic.Fire(new(){associatedCharacter=u},new()));u.Attack=5;callback(0);Check(u.GetStatusEffectStacks(id)==1,"Action raises attack from zero before native checks");u.AddStatusEffect(id,2,new());callback(1);Check(u.GetStatusEffectStacks(id)==3,"new stacks during Multistrike stay for later turns");}),u));}
 {var u=new CharacterState{HP=8};const string id="mt2_succclan.Plugin_frantic";u.Status[id]=2;object?[] args={u,null};Call(typeof(FranticNativeTurnPatch),"Before",args);var callback=(Action<int>)args[1]!;callback(0);Check(u.GetStatusEffectStacks(id)==2,"callback outside own native scope does not consume");
  Drain(FranticNativeTurn.Run(Do(()=>{callback(0);Check(u.GetStatusEffectStacks(id)==2,"inactive normal attack does not consume");}),u));}
 {var u=new CharacterState{HP=8};const string id="mt2_succclan.Plugin_frantic";u.Status[id]=2;object?[] args={u,(Action<int>)(_=>u.Destroyed=true)};Call(typeof(FranticNativeTurnPatch),"Before",args);var callback=(Action<int>)args[1]!;
  Drain(FranticNativeTurn.Run(Do(()=>{Drain(frantic.Fire(new(){associatedCharacter=u},new()));callback(0);Check(u.Status[id]==1,"consume before external callback can destroy attacker");}),u));Check(!FranticNativeTurn.IsActive(u),"destroyed attacker scope cleaned");}
 {var u=new CharacterState{HP=8};u.Status["mt2_succclan.Plugin_frantic"]=2;u.Point.Room.Units=new(){u,new CharacterState{HP=8}};
  Drain(FranticNativeTurn.Run(Do(()=>{Drain(frantic.Fire(new(){associatedCharacter=u},new()));object?[] args={u,new CharacterManager(),null};Call(typeof(FranticNativeTargetsPatch),"Before",args);Check(ReferenceEquals(((List<CharacterState>)args[2]!)[0],u),"front Frantic unit targets itself even with another ally behind");}),u));}
 var own=new CardEffectState();var boss=new CharacterState{Boss=true};var companionBoss=new CharacterState{Companion=true};var minion=new CharacterState{HP=1};
 foreach(var target in new[]{boss,companionBoss,new CharacterState{Destroyed=true},minion}){var list=new List<CharacterState>{target};Call(typeof(SuccClanDropTargetBossPatch),"After",own,list);Check((list.Count==1)==ReferenceEquals(target,minion),"native NULL receives no bosses/destroyed targets");}
 {var p=new CardEffectParams{selfTarget=new CharacterState{Destroyed=true},targets=new(){companionBoss}};own.Card=null;Call(typeof(SuccClanNonBossTargeting),"AfterCollect",own,p);Check(true,"destroyed source never queried");own.Card=new CardState{Id=Guid.NewGuid().ToString(),Asset="Card_othermod_A",ClassId="other-clan-guid"};p.targets=new(){companionBoss};Call(typeof(SuccClanNonBossTargeting),"AfterCollect",own,p);Check(p.targets.Count==1,"other mod unaffected");}

 {var core=new Core();var a=new CardState();var b=new CardState();core.Cards.Hand=new(){a,b};var e=new CardEffectState{Mode=TargetMode.Hand,Effect=new Conductor.CardEffects.CardEffectRandomDiscardFromCardPile()};var p=new CardEffectParams();p.targets.Add(new CharacterState{Destroyed=true});p.targetCards.Add(new CardState());Check((bool)Call(typeof(SuccClanHandCardTargetsPatch),"Before",e,p,core)! == false,"own hand adapter handles missing native case");Check(p.targets.Count==0,"no character targets for hand");Check(p.targetCards.SequenceEqual(core.Cards.Hand),"native hand passed unfiltered for Conductor");Call(typeof(SuccClanHandCardTargetsPatch),"Before",e,p,core);Check(p.targetCards.Count==2,"no duplicate hand candidates on repeated tests");e.Card=new CardState{Id=Guid.NewGuid().ToString(),Asset="Card_othermod_X",ClassId="other-clan-guid"};Check((bool)Call(typeof(SuccClanHandCardTargetsPatch),"Before",e,p,core)! == true,"other mods untouched");e.Card=null;p.selfTarget=new CharacterState();Check((bool)Call(typeof(SuccClanHandCardTargetsPatch),"Before",e,p,core)! == false,"unit trigger supported");p.selfTarget.Destroyed=true;Check((bool)Call(typeof(SuccClanHandCardTargetsPatch),"Before",e,p,core)! == true,"destroyed unit untouched");e.Card=new CardState();e.Mode=TargetMode.Room;Check((bool)Call(typeof(SuccClanHandCardTargetsPatch),"Before",e,p,core)! == true,"non-hand targets untouched");}
 {var card=new CardState();Check(Guid.TryParse(card.GetCardDataID(),out _),"realistic opaque card GUID");Check(SuccClanEffectContext.IsCard(card),"clan card with GUID detected");var saved=Plugin.ClanInstance;Plugin.ClanInstance=null;Check(SuccClanEffectContext.IsCard(card),"asset fallback works before clan callback");var foreign=new CardState{Asset="Card_othermod_X",ClassId="foreign"};Check(!SuccClanEffectContext.IsCard(foreign),"null clan never matches foreign card");Plugin.ClanInstance=saved;var source=new CharacterState{Card=foreign};Check(SuccClanEffectContext.IsCharacter(source),"cardless/source-character asset fallback");source.Data.Asset="Character_othermod_X";Check(!SuccClanEffectContext.IsCharacter(source),"foreign character remains foreign");source.Destroyed=true;Check(!SuccClanEffectContext.IsCharacter(source),"destroyed source ignored before asset access");}
{
 var core=new Core();var card=new CardState();card.Effects.Add(new CardEffectState{Cost=15,Card=card});var trait=new CardTraitBlightHandDamage{Card=card};var stats=new CardStatistics();AllGameManagers.Instance=new(){Cards=core.Cards};
 foreach(int n in new[]{0,1,3}){
  core.Cards.Hand=Enumerable.Range(0,n).Select(_=>new CardState{Type=CardType.Blight}).ToList();core.Cards.Hand.Add(new CardState{Type=CardType.Junk});
  int expected=15+n*20;Check(trait.OnApplyingDamage(new(){damage=15,damageSourceCard=card},core)==expected,"Power Siphon adds magic bonus once and counts only Blights");
  Check(trait.GetCurrentEffectText(stats,null,null).EndsWith(":"+expected),"Power Siphon preview equals actual damage for current hand");
 }
 stats.InBattle=false;Check(!trait.GetCurrentEffectText(stats,null,null).Contains(":75"),"no stale hand total on map");
 core.Cards.Hand=new(){new CardState{Type=CardType.Blight},new CardState{Type=CardType.Blight},new CardState{Type=CardType.Junk}};int discarded=0;core.Cards.OnDiscard=_=>{discarded++;core.Cards.Hand.Add(new CardState{Type=CardType.Blight});};
 Drain(new CardEffectDiscardAllBlights().ApplyEffect(new(),new(){playedCard=card},core,null!));Check(discarded==2,"discard snapshots original Blights");Check(core.Cards.Hand.Count(c=>c.Type==CardType.Blight)==2,"Accursed generated Sparks retained");Check(core.Cards.Hand.Any(c=>c.Type==CardType.Junk),"Scourges not discarded by Power Siphon");AllGameManagers.Instance=null;
}
Console.WriteLine($"PASS {checks} behavioral assertions against actual patch source. Engine lifecycle is simulated, not a Unity playtest.");
 }
}

// 2026-10-27-1459||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Program.cs||Crear banco de pruebas del código real con sustitutos del motor

// 2026-10-27-1540||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Program.cs||Ampliar pruebas del puente de mano Conductor y aislamiento entre mods
// 2026-10-27-1545||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Program.cs||Comprobar frontera de selección nativa tras retirar parche global de TestEffect

// 2026-10-27-2302||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Program.cs||Añadir regresiones H3/H4: Sap0, no ataque/objetivos, Action, Multistrike, callbacks previos y autoataque delantero
// 2026-11-05-1728||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Program.cs||Añadir regresion de identidad GUID, callback de clan aun no disponible y aislamiento de otros mods
// 2026-11-05-1734||codex-succclan-targetless||salidas/build/succclan-bugfixes-2026-10-27/Program.cs||Cubrir daño y preview con 0/1/3 Blights, bonus unico, mapa y descarte seguro frente a generacion Accursed

// 2026-11-07-1205||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\behavior\Program.cs||Consolidar suite local enlazando fuente real del repositorio externo
