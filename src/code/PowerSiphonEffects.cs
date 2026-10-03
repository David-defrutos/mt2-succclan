using System.Collections;

namespace mt2_succclan.Plugin
{
    public sealed class CardTraitBlightHandDamage : CardTraitState
    {
        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();
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
