using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    /// <summary>Integrate Spark with native simultaneous Reserve resolution.</summary>
    [HarmonyPatch(typeof(BalanceData), nameof(BalanceData.GetCardsThatResolveSimultaneouslyOnUnplayed))]
    internal static class ObsessingSparkNativeIntegration
    {
        private static readonly FieldInfo SimultaneousCards = AccessTools.Field(
            typeof(BalanceData), "cardsThatResolveSimultaneouslyOnUnplayed");

        [HarmonyPostfix]
        private static void AfterGetSimultaneousCards(BalanceData __instance,
            ref IReadOnlyList<CardData> __result)
        {
            // JSON registers Spark in the native full-description pool. Read
            // that finalized pool rather than partially loaded card data.
            var pool = __instance.GetBlightsWithFullTooltipDescriptionPool();
            if (pool == null) return;
            string id = MyPluginInfo.PLUGIN_GUID + "-Card-ObsessingShard";
            CardData? spark = null;
            for (int i = 0; i < pool.GetNumCards(); i++)
            {
                var card = pool.GetCardAtIndex(i);
                if (card != null && string.Equals(card.GetID(), id, StringComparison.Ordinal))
                {
                    spark = card;
                    break;
                }
            }
            if (spark == null) return;
            var cards = (List<CardData>?)SimultaneousCards.GetValue(__instance);
            if (cards == null)
            {
                cards = new List<CardData>();
                SimultaneousCards.SetValue(__instance, cards);
            }
            // Preserve vanilla and other mods' entries; repeated getter calls
            // and pre-existing Spark registrations must not add duplicates.
            if (!cards.Exists(card => card != null && card.GetID() == id)) cards.Add(spark);
            __result = cards;
        }
    }
}
