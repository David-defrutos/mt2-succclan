using System.Collections;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    // Marker retained in JSON for localized Extinguish text and native tooltips.
    // Rebirth executes in CheckDeathCardTriggers before the native death cleanup.
    public sealed class CardEffectKnightmareRebirth : CardEffectBase
    {
        internal const string Psionic = "mt2_succclan.Plugin_psionic";
        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();

        internal static int GetRebirthCost(CardEffectState effect, CharacterState source)
            => effect.GetUnmodifiedParamInt() + source.GetAppliedUpgradeCount(effect.GetParamCardUpgradeData());

        internal static CardEffectState? FindEffect(CharacterState source)
        {
            if (source == null || source.IsDestroyed || source.HasStatusEffect("silenced") || source.HasStatusEffect("muted")) return null;
            foreach (var trigger in source.GetTriggers())
                if (trigger.GetTrigger() == CharacterTriggerData.Trigger.OnDeath)
                    foreach (var effect in trigger.GetEffectStates())
                        if (effect.GetCardEffect() is CardEffectKnightmareRebirth) return effect;
            return null;
        }

        internal static bool CanRebirth(CharacterState source, CardEffectState? effect)
            => source != null && !source.IsDestroyed && source.IsDead && source.GetMaxHP() > 0
                && effect != null && effect.GetParamCardUpgradeData() != null
                && source.GetSpawnPoint(false)?.GetRoomOwner()?.IsRoomEnabled() == true
                && source.GetStatusEffectStacks(Psionic) >= GetRebirthCost(effect, source);

        // Native Reanimate queues OnDeath too. This marker must never clone or
        // resurrect again when that queued notification is resolved.
        public override bool TestEffect(CardEffectState effect, CardEffectParams p, ICoreGameManagers core) => false;
        public override IEnumerator ApplyEffect(CardEffectState effect, CardEffectParams p,
            ICoreGameManagers core, ISystemManagers systems) { yield break; }
    }

    internal static class KnightmareNativeRebirth
    {
        private static readonly Dictionary<CharacterState, CardEffectState> Pending = new();

        internal static IEnumerator Check(IEnumerator original, CharacterState source, bool ignoreTriggers)
        {
            if (source == null || source.IsDestroyed) yield break;
            // Existing Undying has priority and never spends Psionic or grants
            // Endless growth. Once exhausted, Psionic rebirth becomes available.
            var effect = ignoreTriggers || source.GetStatusEffectStacks("undying") > 0
                ? null : CardEffectKnightmareRebirth.FindEffect(source);
            bool prepared = !Pending.ContainsKey(source) && CardEffectKnightmareRebirth.CanRebirth(source, effect);
            if (prepared)
            {
                source.AddStatusEffect("undying", 1, new CharacterState.AddStatusEffectParams(),
                    allowModification: false, allowDualism: false, skipDefaultStatusEffectNotifs: true);
                prepared = source.GetStatusEffectStacks("undying") > 0;
                if (prepared)
                {
                    source.RemoveStatusEffect(CardEffectKnightmareRebirth.Psionic,
                        CardEffectKnightmareRebirth.GetRebirthCost(effect!, source), allowModification: false);
                    Pending.Add(source, effect!);
                }
            }
            try { yield return original; }
            finally { if (prepared) Pending.Remove(source); }
        }

        internal static IEnumerator Revive(IEnumerator original, CharacterState source)
        {
            bool first = true;
            try
            {
                while (original.MoveNext())
                {
                    // Native revival has now restored HP=1 and reset its death
                    // flags. Add battle growth and full HP BEFORE Harvest/triggers.
                    if (first && source != null && !source.IsDestroyed
                        && Pending.TryGetValue(source, out var effect))
                    {
                        first = false;
                        Pending.Remove(source);
                        var upgrade = new CardUpgradeState();
                        upgrade.Setup(effect.GetParamCardUpgradeData());
                        yield return source.ApplyCardUpgrade(upgrade, fromSpawn: false,
                            upgradeId: upgrade.GetCardUpgradeDataId());
                        if (!source.IsDestroyed)
                        {
                            if (!source.PreviewMode)
                            {
                                var card = source.GetSpawnerCard();
                                if (card != null)
                                {
                                    card.GetTemporaryCardStateModifiers().AddUpgrade(upgrade);
                                    card.UpdateCardBodyText();
                                }
                            }
                            source.SetHealth(source.GetMaxHP(), source.GetMaxHP());
                        }
                    }
                    yield return original.Current;
                }
            }
            finally { (original as IDisposable)?.Dispose(); }
        }
    }

    [HarmonyPatch(typeof(CharacterState), nameof(CharacterState.CheckDeathCardTriggers))]
    internal static class KnightmareDeathPatch
    {
        [HarmonyPostfix]
        private static void After(CharacterState __instance, bool ignoreTriggers, ref IEnumerator __result)
            => __result = KnightmareNativeRebirth.Check(__result, __instance, ignoreTriggers);
    }

    [HarmonyPatch(typeof(CharacterState), nameof(CharacterState.ReviveFromUndyingStatus))]
    internal static class KnightmareRevivePatch
    {
        [HarmonyPostfix]
        private static void After(CharacterState __instance, ref IEnumerator __result)
            => __result = KnightmareNativeRebirth.Revive(__result, __instance);
    }

    // Own death must not refund Psionic and make Burst 2 behave like Burst 1.
    public sealed class CardEffectKnightmareHarvest : CardEffectAddStatusEffect
    {
        public override bool TestEffect(CardEffectState effect, CardEffectParams p, ICoreGameManagers core)
            => p.selfTarget != null && !p.selfTarget.IsDestroyed && p.dyingCharacter != p.selfTarget
                && base.TestEffect(effect, p, core);
        public override IEnumerator ApplyEffect(CardEffectState effect, CardEffectParams p,
            ICoreGameManagers core, ISystemManagers systems)
        {
            if (TestEffect(effect, p, core)) yield return base.ApplyEffect(effect, p, core, systems);
        }
    }

    [HarmonyPatch(typeof(CardTextReplacements), nameof(CardTextReplacements.GetCardTextReplacementIntegerValue))]
    internal static class KnightmareRebirthCostText
    {
        [HarmonyPostfix]
        private static void After(ILocalizationEffectPower localizationEffectPower, CharacterState characterContext,
            TranslationToken token, ref int? __result)
        {
            if (characterContext == null || characterContext.IsDestroyed
                || localizationEffectPower is not CharacterTriggerState trigger
                || token is not TranslationTokenCardText text || text.elements.Count != 1) return;
            var element = text.elements[0];
            if (element.ElementType != TranslationTokenCardTextElement.TranlationTokenCardTextElementType.Effect
                || element.Index != 0 || element.SubType != TranslationTokenCardTextElement.ParamSubType.Power) return;
            var effect = trigger.GetEffectStates().FirstOrDefault();
            if (effect?.GetCardEffect() is CardEffectKnightmareRebirth)
                __result = CardEffectKnightmareRebirth.GetRebirthCost(effect, characterContext);
        }
    }
}
// 2026-10-27-1456||codex-succclan-targetless||src/code/CardEffectKnightmareRebirth.cs||Knightmare usa Reanimate nativo sobre la misma unidad y carta, curación completa, coste dinámico y sin reembolso por muerte propia
// 2026-10-27-1504||codex-succclan-targetless||src/code/CardEffectKnightmareRebirth.cs||No gastar Psionic si la unidad no puede recibir el puente Undying
