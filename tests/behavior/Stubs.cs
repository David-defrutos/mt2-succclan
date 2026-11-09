// Documento generado el 2026-11-07-1205
// Documento generado el 2026-10-27-1459

using System.Collections;
namespace HarmonyLib {
 [AttributeUsage(AttributeTargets.Class)] public class HarmonyPatch:Attribute { public HarmonyPatch(Type t,string n){} public HarmonyPatch(Type t,string n,Type[] a){} }
 [AttributeUsage(AttributeTargets.Method)] public class HarmonyPostfix:Attribute{}
 [AttributeUsage(AttributeTargets.Method)] public class HarmonyPrefix:Attribute{}
}
public enum TargetMode { Self,DropTargetCharacter,LastTargetedCharacters,FrontInRoom,Room,Hand }
public class Team { public enum Type {None, Monsters, Heroes} }
public class PropDescriptions{}
public interface ICoreGameManagers{CardManager GetCardManager();}
public class CardManager {public List<CardState> Hand=new();public List<CardState> GetHand(bool copy=false)=>copy?Hand.ToList():Hand;public Action<CardState>? OnDiscard;public class DiscardCardParams {public CardState discardCard=null!;public bool triggeredByCard,wasPlayed;public CardState? triggeredCard;}public System.Collections.IEnumerator DiscardCard(DiscardCardParams p){Hand.Remove(p.discardCard);OnDiscard?.Invoke(p.discardCard);yield break;}}
public class Core:ICoreGameManagers{public CardManager Cards=new();public CardManager GetCardManager()=>Cards;}
public interface ISystemManagers{}
public interface ICharacterManager{}
public class CharacterManager:ICharacterManager{}
public class Room {public bool Enabled=true; public List<CharacterState> Units=new();public bool IsRoomEnabled()=>Enabled;
 public void AddCharactersToList(List<CharacterState> list,Team.Type team)=>list.AddRange(Units.Where(x=>x.Team==team));}
public class SpawnPoint {public Room Room=new(); public CharacterState? Unit; public Room GetRoomOwner()=>Room; public CharacterState? GetCharacterState()=>Unit;}
public class CardModifiers {public List<CardUpgradeState> Upgrades=new();public void AddUpgrade(CardUpgradeState u)=>Upgrades.Add(u);}
public class CardState {public string Id=Guid.NewGuid().ToString();public string Asset="mt2_succclan.Plugin-Card-Knightmare";public CardType Type=CardType.Spell;public CardType GetCardType()=>Type;public List<CardEffectState> Effects=new();public List<CardEffectState> GetEffectStates()=>Effects;public string ClassId="succ-clan-guid";public string GetAssetName()=>Asset;public string GetLinkedClassID()=>ClassId;public CardModifiers Mods=new();public string GetCardDataID()=>Id;public CardModifiers GetTemporaryCardStateModifiers()=>Mods;public void UpdateCardBodyText(){} }
public class CharacterData {public string Id=Guid.NewGuid().ToString();public string Asset="mt2_succclan.Plugin-Character-Knightmare";public string GetAssetKey()=>Asset;public string GetID()=>Id;}
public class CardUpgradeData {public string Id="growth";public int Tier=1;public string GetID()=>Id;}
public class CardUpgradeState {public CardUpgradeData Data=null!;public void Setup(CardUpgradeData d)=>Data=d;public string GetCardUpgradeDataId()=>Data.Id;}
public class CharacterTriggerData {public enum Trigger{ OnDeath, Other } }
public interface ILocalizationEffectPower{}
public class CharacterTriggerState:ILocalizationEffectPower { public CharacterTriggerData.Trigger Trigger=CharacterTriggerData.Trigger.OnDeath;public List<CardEffectState> Effects=new();public CharacterTriggerData.Trigger GetTrigger()=>Trigger;public IReadOnlyList<CardEffectState> GetEffectStates()=>Effects;}
public class CharacterState {
 public bool Destroyed;public bool IsDestroyed=>Destroyed;public bool PreviewMode;public bool Boss,Companion;public bool IsDead=>HP<=0;public bool IsAlive=>HP>0;
 public int HP=0,MaxHP=10,Attack=3;public Team.Type Team=global::Team.Type.Monsters;public Dictionary<string,int> Status=new();public List<CardUpgradeState> Upgrades=new();public List<CharacterTriggerState> Triggers=new();public SpawnPoint Point=new();public CardState Card=new();public CharacterData Data=new();public CharacterManager Manager=new();
 public void Guard(){if(Destroyed)throw new Exception("destroyed access");} public int GetHP(){Guard();return HP;} public int GetMaxHP(){Guard();return MaxHP;} public int GetAttackDamage(){Guard();return Attack;} public bool IsAnyBoss(){Guard();return Boss;}public bool IsCompanionBoss(){Guard();return Companion;}public Team.Type GetTeamType(){Guard();return Team;}
 public class AddStatusEffectParams{}
 public int GetStatusEffectStacks(string id){Guard();return Status.GetValueOrDefault(id);} public bool HasStatusEffect(string id)=>GetStatusEffectStacks(id)>0;
 public void RemoveStatusEffect(string id,int count,bool allowModification=true){Guard();Status[id]=Math.Max(0,GetStatusEffectStacks(id)-count);}
 public void AddStatusEffect(string id,int count,AddStatusEffectParams p,bool allowModification=true,bool allowDualism=true,bool skipDefaultStatusEffectNotifs=false){Guard();Status[id]=GetStatusEffectStacks(id)+count;}
 public int GetAppliedUpgradeCount(CardUpgradeData d){Guard();return Upgrades.Count(u=>u.Data.Id==d.Id);} public List<CharacterTriggerState> GetTriggers(){Guard();return Triggers;}
 public SpawnPoint GetSpawnPoint(bool any=false){Guard();return Point;} public CardState GetSpawnerCard(){Guard();return Card;}public CharacterData GetSourceCharacterData(){Guard();return Data;}public ICharacterManager GetCharacterManager(){Guard();return Manager;}
 public IEnumerator ApplyCardUpgrade(CardUpgradeState u,bool fromSpawn=false,string upgradeId=""){Guard();Upgrades.Add(u);MaxHP+=u.Data.Tier;Attack+=u.Data.Tier;Status["multistrike"]=GetStatusEffectStacks("multistrike")+1;yield break;}
 public void SetHealth(int hp,int max){Guard();HP=hp;MaxHP=max;}
 public IEnumerator CheckDeathCardTriggers()=>Empty();public IEnumerator ReviveFromUndyingStatus()=>Empty();static IEnumerator Empty(){yield break;}
}
public class CardEffectParams { public CharacterState? selfTarget,dyingCharacter,overrideTargetCharacter,characterThatActivatedAbility;public CardState? playedCard;public SpawnPoint? dropLocation;public List<CharacterState> targets=new();public List<CardState> targetCards=new(); }
public abstract class CardEffectBase {public virtual bool CanPlayAfterBossDead=>true;public virtual bool CanApplyInPreviewMode=>true;public virtual PropDescriptions CreateEditorInspectorDescriptions()=>new();public virtual bool TestEffect(CardEffectState e,CardEffectParams p,ICoreGameManagers c)=>true;public abstract IEnumerator ApplyEffect(CardEffectState e,CardEffectParams p,ICoreGameManagers c,ISystemManagers s);}
public class CardEffectAddStatusEffect:CardEffectBase {public override IEnumerator ApplyEffect(CardEffectState e,CardEffectParams p,ICoreGameManagers c,ISystemManagers s){p.selfTarget!.Status["harvest"]=p.selfTarget.Status.GetValueOrDefault("harvest")+1;yield break;}}
public class CardEffectState {public CardEffectData SourceData=new();public CardEffectData GetSourceCardEffectData()=>SourceData;public int Cost=2;public CardUpgradeData Upgrade=new();public CardEffectBase Effect=null!;public bool Ignore=true;public TargetMode Mode=TargetMode.DropTargetCharacter;public CardState? Card=new();public int GetParamInt()=>Cost;public int GetUnmodifiedParamInt()=>Cost;public CardUpgradeData GetParamCardUpgradeData()=>Upgrade;public CardEffectBase GetCardEffect()=>Effect;public bool GetTargetIgnoreBosses()=>Ignore;public CardState? GetParentCardState()=>Card;public TargetMode GetTargetMode()=>Mode;}
public abstract class StatusEffectState {
 public CharacterState? Associated; public string GetStatusId()=>"mt2_succclan.Plugin_frantic";public CharacterState? GetAssociatedCharacter()=>Associated;
 public class InputTriggerParams{public CharacterState? associatedCharacter;}public class OutputTriggerParams{public bool canAttackOrHeal=true;}
 public abstract bool TestTrigger(InputTriggerParams i,OutputTriggerParams o,ICoreGameManagers c);protected abstract IEnumerator OnTriggered(InputTriggerParams i,OutputTriggerParams o,ICoreGameManagers c);public abstract int GetEffectMagnitude(int stacks=1);
 public IEnumerator Fire(InputTriggerParams i,OutputTriggerParams o)=>OnTriggered(i,o,null!);
}
public class CombatManager {public void RunUnitTurn(){} }
public static class TargetHelper { public struct CollectTargetsData{public TargetMode targetMode;}public static IReadOnlyList<CharacterState> LastTargetedCharacters=new List<CharacterState>();public static void CollectTargets(){}public static void CheckTargetsOverride(){} }
public static class GameEffectHelper {public static void TestEffect(){} }
public class TranslationToken{}
public class TranslationTokenCardText:TranslationToken {public List<TranslationTokenCardTextElement> elements=new();}
public class TranslationTokenCardTextElement {public enum TranlationTokenCardTextElementType{Effect,Status}public enum ParamSubType{Power,Other}public TranlationTokenCardTextElementType ElementType;public ParamSubType SubType;public int Index;}
public static class CardTextReplacements {public static void GetCardTextReplacementIntegerValue(){} }
namespace mt2_succclan.Plugin {public static class MyPluginInfo {public const string PLUGIN_GUID="mt2_succclan.Plugin";}public static class FranticBossImmunity {public static bool IsBoss(CharacterState c)=>c.Boss||c.Companion;} }

// 2026-10-27-1459||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Stubs.cs||Crear banco de pruebas del código real con sustitutos del motor

// 2026-10-27-1500||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Stubs.cs||Corregir ambigüedad de Team en sustituto del motor

namespace Conductor.CardEffects {public class CardEffectRandomDiscardFromCardPile:CardEffectBase {public override IEnumerator ApplyEffect(CardEffectState e,CardEffectParams p,ICoreGameManagers c,ISystemManagers s){yield break;}}}

// 2026-10-27-1540||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Stubs.cs||Ampliar pruebas del puente de mano Conductor y aislamiento entre mods

public class ClassData {public string GetID()=>"succ-clan-guid";}
namespace mt2_succclan.Plugin {public static class Plugin {internal static ClassData? ClanInstance=new();}}
namespace TrainworksReloaded.Base {public static class TemplateConstants {public const string Card="Card",Character="Character";}}
namespace TrainworksReloaded.Base.Extensions {public static class AssetExtension {public static bool BelongsToMod(this string value,string mod,string template)=>value.Contains(mod,StringComparison.Ordinal);}}
// 2026-11-05-1728||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Stubs.cs||Usar GUID opacos y asset keys separados; modelar identidad de clan y habilidad activada

public enum CardType {Spell,Monster,Blight,Junk}
public interface ICardEffectDiscards{}
public class CardStatistics {public bool InBattle=true;public bool GetIsInActiveBattle()=>InBattle;}
public class SaveManager {}
public class RelicManager {}
public class AllGameManagers {public static AllGameManagers? Instance;public CardManager? Cards;public CardManager? GetCardManager()=>Cards;}
public class CardTraitState {public CardState Card=new();public int Power=20;public CardState GetCard()=>Card;public int GetParamInt()=>Power;public virtual PropDescriptions CreateEditorInspectorDescriptions()=>new();public virtual bool HasMultiWordDesc()=>false;public virtual string GetCurrentEffectText(CardStatistics? s,SaveManager? v,RelicManager? r)=>"";public class ApplyingDamageParameters {public int damage;public CardState? damageSourceCard;}public virtual int OnApplyingDamage(ApplyingDamageParameters d,ICoreGameManagers c)=>d.damage;}
public class LocalizedIntegers {public int Value;public LocalizedIntegers(int n)=>Value=n;}
public static class LocalizeMock {public static string Localize(this string key)=>key;public static string Localize(this string key,LocalizedIntegers n)=>key+":"+n.Value;}
// 2026-11-05-1734||codex-succclan-targetless||salidas/build/succclan-bugfixes-2026-10-27/Stubs.cs||Ampliar sustitutos para probar daño real y preview Power Siphon, descarte con nuevos Sparks
// 2026-11-05-1738||codex-succclan-targetless||salidas\build\succclan-bugfixes-2026-10-27\Stubs.cs||Contrastar sustituto BelongsToMod con API real y usar asset names separados de GUID

// 2026-11-07-1205||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\tests\behavior\Stubs.cs||Consolidar suite local enlazando fuente real del repositorio externo

public class CardEffectData {public AdditionalTooltipData[] Tooltips=Array.Empty<AdditionalTooltipData>();public CharacterTriggerData.Trigger Trigger;public AdditionalTooltipData[] GetAdditionalTooltips()=>Tooltips;public CharacterTriggerData.Trigger GetParamTrigger()=>Trigger;}
public class AdditionalTooltipData {public string titleKey="";}
public struct TooltipContent {public string tooltipId,title,body;}
public class CardTooltipHelper {public static void GetCardEffectTooltipContents(){}}
public class TooltipUI {public enum TitleStyle {Normal}}

// 2026-11-09-2140||codex-succclan-targetless||repos/MT2/variaciones-mias/mt2-succclan/tests/behavior/Stubs.cs||Modelar solo estructuras de tooltip para probar aislamiento y disposicion de enumerador
