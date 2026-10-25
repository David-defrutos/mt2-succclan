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
            var card = effectState.GetParentCardState() ?? cardEffectParams.playedCard;
            var cardId = card?.GetCardDataID();
            var sourceId = cardEffectParams.selfTarget?.GetSourceCharacterData()?.GetID();
            bool ownCard = cardId != null && cardId.StartsWith(MyPluginInfo.PLUGIN_GUID + "-", StringComparison.Ordinal);
            bool ownUnit = sourceId != null && sourceId.StartsWith(MyPluginInfo.PLUGIN_GUID + "-", StringComparison.Ordinal);
            if (!ownCard && !ownUnit) return;
            cardEffectParams.targets?.RemoveAll(IsCompanionBoss);
            if (TargetHelper.LastTargetedCharacters is List<CharacterState> previous)
                previous.RemoveAll(IsCompanionBoss);
        }

        private static bool IsCompanionBoss(CharacterState target)
            => target != null && target.IsCompanionBoss();
    }
}
// 2026-10-25-1626||codex-succclan-targetless||src\code\NonBossTargeting.cs||Completar filtro nativo solo para efectos SuccClan con target_ignore_bosses: excluir bosses acompañantes
// 2026-10-25-1627||codex-succclan-targetless||src\code\NonBossTargeting.cs||Acotar por carta o unidad invocadora SuccClan; CardEffectData no posee GetID
