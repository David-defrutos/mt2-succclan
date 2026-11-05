using System.Collections;

namespace mt2_succclan.Plugin
{
    public sealed class CardTraitBlightHandDamage : CardTraitState
    {
        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();
        public override bool HasMultiWordDesc() => true;

        public override string GetCurrentEffectText(CardStatistics? stats, SaveManager? save, RelicManager? relics)
        {
            var card = GetCard();
            if (card == null) return string.Empty;
            // Native effects already include upgraded/room magic power and Attuned.
            var effects = card.GetEffectStates();
            if (effects.Count == 0) return string.Empty;
            int bonus = effects[0].GetParamInt();
            if (stats == null || !stats.GetIsInActiveBattle() || AllGameManagers.Instance == null)
                return bonus > 0 ? string.Format(
                    "CardTraitScalingAddDamage_ExtraDamage_XCostOutsideBattle_CardText".Localize(),
                    bonus, "tempUpgradeHighlight") : string.Empty;
            var manager = AllGameManagers.Instance.GetCardManager();
            if (manager == null) return string.Empty;
            int count = manager.GetHand().Count(
                candidate => candidate != card && candidate.GetCardType() == CardType.Blight);
            return "CardTraitScalingAddDamage_CurrentScaling_CardText".Localize(
                new LocalizedIntegers(bonus + count * GetParamInt()));
        }

        public override int OnApplyingDamage(ApplyingDamageParameters damage, ICoreGameManagers core)
        {
            int count = core.GetCardManager().GetHand().Count(card =>
                card != damage.damageSourceCard && card.GetCardType() == CardType.Blight);
            return damage.damage + count * GetParamInt();
        }
    }

    public sealed class CardEffectDiscardAllBlights : CardEffectBase, ICardEffectDiscards
    {
        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();
        public override bool CanPlayAfterBossDead => false;
        public override bool CanApplyInPreviewMode => false;
        public override IEnumerator ApplyEffect(CardEffectState state, CardEffectParams parameters,
            ICoreGameManagers core, ISystemManagers systems)
        {
            var manager = core.GetCardManager();
            // Snapshot before discarding: Accursed can add new Sparks to the hand.
            var blights = manager.GetHand(true).Where(card => card != parameters.playedCard &&
                card.GetCardType() == CardType.Blight).ToList();
            foreach (var card in blights)
                yield return manager.DiscardCard(new CardManager.DiscardCardParams {
                    discardCard = card, triggeredByCard = true,
                    triggeredCard = parameters.playedCard, wasPlayed = false });
        }
    }
}
// 2026-11-05-1728||codex-succclan-targetless||src\code\PowerSiphonEffects.cs||Mostrar total dinamico de Power Siphon con mejoras y Conduit usando efecto y texto nativos; no modificar daño
// 2026-11-05-1730||codex-succclan-targetless||src\code\PowerSiphonEffects.cs||Proteger CardManager ausente al generar texto de daño fuera de partida
// 2026-11-05-1737||codex-succclan-targetless||src/code/PowerSiphonEffects.cs||Proteger render de carta antes de inicializar sus efectos
