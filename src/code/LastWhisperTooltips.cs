// Documento generado el 2026-11-09-2140
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    // Native replay-trigger tooltip code formats the extended enum as a number.
    // Its hide_tooltip flag does not guard that branch. Only effects explicitly
    // marked by our localized tooltip opt into this presentation-only filter.
    internal static class LastWhisperTooltips
    {
        internal const string TitleKey = "mt2_succclan_LastWhisper_Accursed_Title";

        internal static IEnumerable<TooltipContent> Rewrite(
            IEnumerable<TooltipContent> original, CardEffectData data)
        {
            if (data == null || data.GetAdditionalTooltips() == null
                || !data.GetAdditionalTooltips().Any(t => t != null && t.titleKey == TitleKey))
                return original;
            return WithoutNativeTrigger(original, data.GetParamTrigger().ToString());
        }

        private static IEnumerable<TooltipContent> WithoutNativeTrigger(
            IEnumerable<TooltipContent> original, string nativeTriggerId)
        {
            foreach (var tooltip in original)
                if (tooltip.tooltipId != nativeTriggerId)
                    yield return tooltip;
        }
    }

    [HarmonyPatch(typeof(CardTooltipHelper), nameof(CardTooltipHelper.GetCardEffectTooltipContents),
        new[] { typeof(CardEffectData), typeof(SaveManager) })]
    internal static class LastWhisperDataTooltipPatch
    {
        static void Postfix(CardEffectData cardEffectData, ref IEnumerable<TooltipContent> __result)
        {
            __result = LastWhisperTooltips.Rewrite(__result, cardEffectData);
        }
    }

    [HarmonyPatch(typeof(CardTooltipHelper), nameof(CardTooltipHelper.GetCardEffectTooltipContents),
        new[] { typeof(CardEffectState), typeof(TooltipUI.TitleStyle), typeof(SaveManager) })]
    internal static class LastWhisperStateTooltipPatch
    {
        static void Postfix(CardEffectState cardEffectState, ref IEnumerable<TooltipContent> __result)
        {
            __result = LastWhisperTooltips.Rewrite(__result, cardEffectState.GetSourceCardEffectData());
        }
    }
}

// 2026-11-09-2140||codex-succclan-targetless||repos/MT2/variaciones-mias/mt2-succclan/src/code/LastWhisperTooltips.cs||Filtrar solo tooltip de trigger nativo marcado Last Whisper; enum dinamico sin75 fijo y dos rutas data/state; sin gameplay
