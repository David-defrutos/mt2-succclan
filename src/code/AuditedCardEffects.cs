using System.Collections;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    public sealed class CardEffectPsionicPermanentGrowth : CardEffectAddCardUpgradeToUnits
    {
        public override IEnumerator ApplyEffect(CardEffectState s, CardEffectParams p, ICoreGameManagers c, ISystemManagers m)
        {
            var target = p.selfTarget;
            if (target == null || target.IsDestroyed) yield break;
            var cost = s.GetSourceCardEffectData().GetParamStatusEffects()[0];
            int times = target.GetStatusEffectStacks(cost.statusId) / cost.count;
            // Spend a snapshot once, so upgrades cannot increase the loop's upper bound.
            target.RemoveStatusEffect(cost.statusId, times * cost.count, allowModification: false);
            for (int i = 0; i < times; i++) yield return base.ApplyEffect(s, p, c, m);
        }
    }

    public sealed class RelicEffectRageOnFrantic : RelicEffectBase
    {
        public override bool CanApplyInPreviewMode => true;
        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();
        public override void OnCharacterStatusEffectApplied(CharacterState character, StatusEffectState status,
            ICoreGameManagers core, RelicManager relics, SaveManager save, bool spawnEffect)
        {
            if (character == null || character.IsDestroyed || status.GetStatusId() != "mt2_succclan.Plugin_frantic") return;
            character.AddStatusEffect("buff", 1, new CharacterState.AddStatusEffectParams { sourceRelicState = SourceRelicState });
        }
    }

    public sealed class RelicEffectArmorOnPyreAttacked : RelicEffectBase
    {
        public override bool CanApplyInPreviewMode => true;
        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();
    }


    // Observe the complete native attack resolution instead of an intermediate
    // status stage which may be skipped by early exits. One award per hit,
    // including hits blocked by Armor; Reserve/spell damage does not trigger it.
    [HarmonyPatch(typeof(CharacterState), nameof(CharacterState.ApplyDamage))]
    internal static class PyreAttackedArmorPatch
    {
        private static void Postfix(CharacterState __instance, CharacterState.ApplyDamageParams damageParams,
            RelicManager ___relicManager, ref IEnumerator __result)
        {
            if (__instance == null || __instance.IsDestroyed || !__instance.IsPyreHeart()
                || damageParams.attacker == null || damageParams.attacker.IsDestroyed
                || damageParams.attacker.GetTeamType() != Team.Type.Heroes
                || damageParams.damageType != Damage.Type.DirectAttack) return;
            __result = ApplyAfter(__result, __instance, ___relicManager);
        }
        private static IEnumerator ApplyAfter(IEnumerator original, CharacterState pyre, RelicManager relics)
        {
            yield return original;
            if (pyre == null || pyre.IsDestroyed || !pyre.IsAlive || relics == null) yield break;
            foreach (var effect in relics.GetRelicEffects(new List<RelicEffectArmorOnPyreAttacked>()))
                foreach (var status in effect.SourceRelicEffectData.GetParamStatusEffects())
                    pyre.AddStatusEffect(status.statusId, status.count,
                        new CharacterState.AddStatusEffectParams { sourceRelicState = effect.SourceRelicState,
                            overrideImmunity = true });
        }
    }
}
// 2026-10-27-1456||codex-succclan-targetless||src/code/AuditedCardEffects.cs||Flare Ribbon observa resolución completa del ataque a Pira, sin acceso tras destrucción
// 2026-10-27-1536||codex-succclan-targetless||src/code/AuditedCardEffects.cs||Retirar descarte duplicado y aplicar Armor a Pira con excepción de inmunidad usada por efecto nativo Pyre
