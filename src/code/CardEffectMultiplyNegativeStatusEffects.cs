using System.Collections;
using System.Collections.Generic;

namespace mt2_succclan.Plugin
{
    // The native MultiplyAllStatusEffects also multiplies persistent effects (e.g. Armor).
    public sealed class CardEffectMultiplyNegativeStatusEffects : CardEffectBase
    {
        public override PropDescriptions CreateEditorInspectorDescriptions()
            => new CardEffectMultiplyAllStatusEffects().CreateEditorInspectorDescriptions();

        public override IEnumerator ApplyEffect(CardEffectState effect, CardEffectParams args,
            ICoreGameManagers managers, ISystemManagers systems)
        {
            var add = new CharacterState.AddStatusEffectParams();
            add.ApplyEffectParams<CardEffectMultiplyNegativeStatusEffects>(effect, args);
            var statuses = new List<CharacterState.StatusEffectStack>();
            for (int i = args.targets.Count - 1; i >= 0; i--)
            {
                var target = args.targets[i];
                statuses.Clear();
                target.GetStatusEffects(ref statuses);
                foreach (var status in statuses)
                {
                    if (status.State.GetDisplayCategory() != StatusEffectData.DisplayCategory.Negative)
                        continue;
                    int extra = status.Count * (effect.GetParamInt() - 1);
                    if (extra > 0)
                        target.AddStatusEffect(status.State.GetStatusId(), extra, add, null,
                            allowModification: true, isFromHiddenTrigger: args.isFromHiddenTrigger);
                }
            }
            yield break;
        }
    }
}
