using System.Collections;
using System.Collections.Generic;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    // The native spawner applies its HP penalty AFTER spawning. A zero-HP
    // copy is sacrificed immediately and re-enters OnDeath/RemoveDeadCharacters
    // while the previous death coroutine is still resolving.
    public sealed class CardEffectEndlessShadowRebirth : CardEffectBase
    {
        private const int HealthPenalty = 5;
        private readonly CardEffectSpawnMonster nativeSpawner = new CardEffectSpawnMonster();

        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();

        public override bool TestEffect(CardEffectState effect, CardEffectParams parameters, ICoreGameManagers core)
        {
            var source = parameters.selfTarget;
            if (source == null || source.IsDestroyed || !source.IsDead) return false;
            // Use the same card/character HP the native spawner will construct,
            // rather than battle buffs that the native copy does not inherit.
            float spawnHealth = parameters.playedCard != null && !effect.GetParamBool()
                ? parameters.playedCard.GetHealth()
                : effect.GetParamCharacterData()?.GetHealth() ?? 0;
            return spawnHealth > HealthPenalty
                && nativeSpawner.TestEffect(effect, parameters, core);
        }

        public override IEnumerator ApplyEffect(CardEffectState effect, CardEffectParams parameters,
            ICoreGameManagers core, ISystemManagers systems)
        {
            // Recheck at execution: queued triggers may outlive their source.
            if (!TestEffect(effect, parameters, core)) yield break;
            if (!core.GetSaveManager().PreviewMode)
            {
                yield return nativeSpawner.ApplyEffect(effect, parameters, core, systems);
                yield break;
            }
            var scope = new EndlessShadowPreviewUpgrade.Scope(parameters.playedCard,
                effect.GetParamCardUpgradeData());
            // Keep the scope across nested coroutines; cancellation must clear it.
            EndlessShadowPreviewUpgrade.Scopes.Add(scope);
            try { yield return nativeSpawner.ApplyEffect(effect, parameters, core, systems); }
            finally { EndlessShadowPreviewUpgrade.Scopes.Remove(scope); }
        }
    }

    // SpawnMonster records its HP penalty on the card only outside preview.
    // Supply that bookkeeping on the NEW preview card before the penalty can
    // resolve another death. Native code still applies the unit upgrade once.
    [HarmonyPatch(typeof(CharacterState), nameof(CharacterState.ApplyCardUpgrade))]
    internal static class EndlessShadowPreviewUpgrade
    {
        internal sealed class Scope
        {
            internal readonly CardState? Source;
            internal readonly CardUpgradeData? Upgrade;
            internal readonly HashSet<CardState> Recorded = new();
            internal Scope(CardState? source, CardUpgradeData? upgrade)
            { Source = source; Upgrade = upgrade; }
        }
        internal static readonly List<Scope> Scopes = new();

        [HarmonyPrefix]
        internal static void Before(CharacterState __instance, CardUpgradeState cardUpgradeState, bool fromSpawn)
        {
            if (fromSpawn || __instance == null || __instance.IsDestroyed
                || !__instance.PreviewMode || !__instance.SpawnedInPreviewMode) return;
            var card = __instance.GetSpawnerCard();
            if (card == null) return;
            for (int i = Scopes.Count - 1; i >= 0; i--)
            {
                var scope = Scopes[i];
                if (scope.Source == null || ReferenceEquals(card, scope.Source) || scope.Upgrade == null
                    || cardUpgradeState.GetCardUpgradeDataId() != scope.Upgrade.GetID()) continue;
                if (scope.Recorded.Add(card))
                    card.GetTemporaryCardStateModifiers().AddUpgrade(cardUpgradeState);
                return;
            }
        }
    }
}

// 2026-11-07-1152||codex-succclan-targetless||src/code/CardEffectEndlessShadowRebirth.cs||Sincronizar penalizacion HP en nueva carta de preview con alcance nativo; limpiar al cancelar

// 2026-11-07-1154||codex-succclan-targetless||src/code/CardEffectEndlessShadowRebirth.cs||Acotar bookkeeping a preview; ejecucion real delega directamente en spawner nativo
