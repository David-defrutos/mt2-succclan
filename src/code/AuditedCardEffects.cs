using System.Collections;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    public sealed class CardEffectDiscardSpecificCard : CardEffectBase, ICardEffectDiscards
    {
        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();
        public override bool CanPlayAfterBossDead => false;
        public override bool CanApplyInPreviewMode => false;
        public override bool TestEffect(CardEffectState s, CardEffectParams p, ICoreGameManagers c)
            => c.GetCardManager().GetHand().Count(card => card != p.playedCard &&
                card.GetCardDataID() == s.GetParamCardData().GetID()) >= s.GetParamInt();
        public override IEnumerator ApplyEffect(CardEffectState s, CardEffectParams p, ICoreGameManagers c, ISystemManagers m)
        {
            var manager = c.GetCardManager();
            var id = s.GetParamCardData().GetID();
            var cards = manager.GetHand(true).Where(card => card != p.playedCard && card.GetCardDataID() == id).ToList();
            int count = Math.Min(s.GetParamInt(), cards.Count);
            // Select before discarding: Accursed can create more Sparks during resolution.
            while (cards.Count > count) cards.RemoveAt(RandomManager.Range(0, cards.Count, RngId.Battle));
            foreach (var card in cards)
                yield return manager.DiscardCard(new CardManager.DiscardCardParams {
                    discardCard = card, triggeredByCard = true, triggeredCard = p.playedCard, wasPlayed = false });
        }
    }

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
            if (status.GetStatusId() != "mt2_succclan.Plugin_frantic") return;
            character.AddStatusEffect("buff", 1, new CharacterState.AddStatusEffectParams { sourceRelicState = SourceRelicState });
        }
    }

    public sealed class RelicEffectArmorOnPyreAttacked : RelicEffectBase
    {
        public override bool CanApplyInPreviewMode => true;
        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();
    }

    [HarmonyPatch(typeof(CharacterState), "DoAttackedStatusEffects")]
    internal static class PyreAttackedArmorPatch
    {
        private static void Postfix(CharacterState __instance, CharacterState attacker, RelicManager ___relicManager, ref IEnumerator __result)
        {
            if (!__instance.IsPyreHeart() || attacker == null || attacker.GetTeamType() != Team.Type.Heroes) return;
            __result = ApplyAfter(__result, __instance, ___relicManager);
        }
        private static IEnumerator ApplyAfter(IEnumerator original, CharacterState pyre, RelicManager relics)
        {
            yield return original;
            foreach (var effect in relics.GetRelicEffects(new List<RelicEffectArmorOnPyreAttacked>()))
                foreach (var status in effect.SourceRelicEffectData.GetParamStatusEffects())
                    pyre.AddStatusEffect(status.statusId, status.count,
                        new CharacterState.AddStatusEffectParams { sourceRelicState = effect.SourceRelicState });
        }
    }
}
