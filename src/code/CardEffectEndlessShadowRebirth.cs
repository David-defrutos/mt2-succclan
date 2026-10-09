using System.Collections;

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
            yield return nativeSpawner.ApplyEffect(effect, parameters, core, systems);
        }
    }
}
