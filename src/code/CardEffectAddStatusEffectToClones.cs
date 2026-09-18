using System.Collections;

namespace mt2_succclan.Plugin
{
    /// <summary>
    /// Legacy effect retained for compatibility. Illusion Twins now uses
    /// CardEffectCopyWithStatuses, which identifies the new copy by callback and
    /// applies Fragile after the engine has finished copying its states.
    /// </summary>
    public class CardEffectAddStatusEffectToClones : CardEffectBase
    {
        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers,
            ISystemManagers sysManagers)
        {
            StatusEffectStackData[] estados = cardEffectState.GetSourceCardEffectData().GetParamStatusEffects();
            if (estados == null || estados.Length == 0)
            {
                yield break;
            }

            foreach (CharacterState unidad in cardEffectParams.targets)
            {
                if (unidad == null || !unidad.GetIsClone())
                {
                    continue;
                }

                foreach (StatusEffectStackData estado in estados)
                {
                    if (estado == null || estado.count <= 0)
                    {
                        continue;
                    }

                    // Ya lo tiene: no acumular.
                    if (unidad.GetStatusEffectStacks(estado.statusId) > 0)
                    {
                        continue;
                    }

                    unidad.AddStatusEffect(
                        addStatusEffectParams: new CharacterState.AddStatusEffectParams
                        {
                            sourceRelicState = cardEffectParams.sourceRelic,
                            sourceIsHero = (cardEffectState.GetSourceTeamType() == Team.Type.Heroes),
                            fromEffectType = this.GetType(),
                        },
                        statusId: estado.statusId,
                        numStacks: estado.count,
                        allowModification: false,
                        isFromHiddenTrigger: cardEffectParams.isFromHiddenTrigger);
                }
            }
        }

        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions
            {
                [CardEffectFieldNames.ParamStatusEffects.GetFieldName()] =
                    new PropDescription("Status Effects", "Estados que se aplican, y solo, a las unidades marcadas como clon."),
            };
        }
    }
}
