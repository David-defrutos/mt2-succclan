// Documento generado el 2026-10-25-1626
using System;
using System.Collections.Generic;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    // Native target_ignore_bosses handles IsAnyBoss, but not companion bosses.
    [HarmonyPatch(typeof(TargetHelper), nameof(TargetHelper.CollectTargets),
        new Type[] { typeof(CardEffectState), typeof(CardEffectParams), typeof(ICoreGameManagers), typeof(bool) })]
    internal static class SuccClanNonBossTargeting
    {
        [HarmonyPostfix]
        private static void AfterCollect(CardEffectState effectState, CardEffectParams cardEffectParams)
        {
            if (effectState == null || !effectState.GetTargetIgnoreBosses()) return;
            if (!SuccClanEffectContext.IsOwn(effectState, cardEffectParams)) return;
            cardEffectParams.targets?.RemoveAll(IsCompanionBoss);
            if (TargetHelper.LastTargetedCharacters is List<CharacterState> previous)
                previous.RemoveAll(IsCompanionBoss);
        }

        private static bool IsCompanionBoss(CharacterState target)
            => target != null && (target.IsDestroyed || target.IsCompanionBoss());
    }
}
// 2026-10-25-1626||codex-succclan-targetless||src\code\NonBossTargeting.cs||Completar filtro nativo solo para efectos SuccClan con target_ignore_bosses: excluir bosses acompañantes
// 2026-10-25-1627||codex-succclan-targetless||src\code\NonBossTargeting.cs||Acotar por carta o unidad invocadora SuccClan; CardEffectData no posee GetID

namespace mt2_succclan.Plugin
{
    internal static class SuccClanBossTargetValidation
    {
        internal static bool Applies(CardEffectState effect)
        {
            var id = effect?.GetParentCardState()?.GetCardDataID();
            return effect != null && effect.GetTargetIgnoreBosses() && id != null
                && id.StartsWith(MyPluginInfo.PLUGIN_GUID + "-", StringComparison.Ordinal);
        }
        internal static bool Invalid(CharacterState? target)
            => target == null || target.IsDestroyed || target.IsAnyBoss() || target.IsCompanionBoss();
    }

    // Native override targeting runs AFTER CollectTargets and can reintroduce a
    // companion boss. Filter at that boundary as well, for both UI and execution.
    [HarmonyLib.HarmonyPatch(typeof(TargetHelper), nameof(TargetHelper.CheckTargetsOverride))]
    internal static class SuccClanDropTargetBossPatch
    {
        [HarmonyLib.HarmonyPostfix]
        private static void After(CardEffectState effectState, List<CharacterState> targets)
        {
            if (!SuccClanBossTargetValidation.Applies(effectState)) return;
            targets.RemoveAll(SuccClanBossTargetValidation.Invalid);
            if (TargetHelper.LastTargetedCharacters is List<CharacterState> previous)
                previous.RemoveAll(SuccClanBossTargetValidation.Invalid);
        }
    }


}
// 2026-10-27-1456||codex-succclan-targetless||src/code/NonBossTargeting.cs||Validar bosses en override y test de juego; evitar consultar fuente destruida
// 2026-10-27-1458||codex-succclan-targetless||src/code/NonBossTargeting.cs||Anotar como opcional el objetivo ausente durante validación

namespace mt2_succclan.Plugin
{
    internal static class SuccClanEffectContext
    {
        internal static bool IsOwn(CardEffectState effect, CardEffectParams parameters)
        {
            var id = (effect.GetParentCardState() ?? parameters.playedCard)?.GetCardDataID();
            if (id != null && id.StartsWith(MyPluginInfo.PLUGIN_GUID + "-", StringComparison.Ordinal)) return true;
            var unit = parameters.selfTarget;
            if (unit == null || unit.IsDestroyed) return false;
            id = unit.GetSourceCharacterData()?.GetID();
            return id != null && id.StartsWith(MyPluginInfo.PLUGIN_GUID + "-", StringComparison.Ordinal);
        }
    }

    // Conductor owns pool filtering, random selection, count validation and
    // discard. MT2's collector omits Hand, so only provide that missing input.
    [HarmonyPatch(typeof(TargetHelper), nameof(TargetHelper.CollectTargets),
        new Type[] { typeof(CardEffectState), typeof(CardEffectParams), typeof(ICoreGameManagers), typeof(bool) })]
    internal static class SuccClanHandCardTargetsPatch
    {
        [HarmonyPrefix]
        private static bool Before(CardEffectState effectState, CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers)
        {
            if (coreGameManagers == null || effectState == null || effectState.GetTargetMode() != TargetMode.Hand
                || effectState.GetCardEffect()?.GetType().FullName != "Conductor.CardEffects.CardEffectRandomDiscardFromCardPile"
                || !SuccClanEffectContext.IsOwn(effectState, cardEffectParams)) return true;
            cardEffectParams.targets.Clear();
            cardEffectParams.targetCards.Clear();
            cardEffectParams.targetCards.AddRange(coreGameManagers.GetCardManager().GetHand());
            return false;
        }
    }
}
// 2026-10-27-1540||codex-succclan-targetless||src/code/NonBossTargeting.cs||Conectar mano nativa con descarte Conductor solo para SuccClan y compartir comprobación segura de contexto
// 2026-10-27-1545||codex-succclan-targetless||src\code\NonBossTargeting.cs||Retirar parche global de TestEffect: validación sustituida por CardEffectNULL nativo
// 2026-10-27-1547||codex-succclan-targetless||src/code/NonBossTargeting.cs||Mantener comprobación de contexto nulo en puente nativo de mano
