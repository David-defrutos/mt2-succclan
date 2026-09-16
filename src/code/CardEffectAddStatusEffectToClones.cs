using System.Collections;

namespace mt2_succclan.Plugin
{
    /// <summary>
    /// Aplica estados solo a las unidades que son CLONES, saltandose las demas.
    /// Pensado para ir justo detras de un CardEffectCopyUnits en la misma carta.
    ///
    /// Por que hace falta (16-sep-2026):
    ///   - CardEffectCopyUnits.Setup solo lee GetParamInt, o sea que IGNORA param_upgrade
    ///     en silencio. Ponerlo ahi no da error y no hace nada.
    ///   - CardEffectCopyUnits es SEALED, asi que no se puede heredar de el para envolverlo.
    ///   - MonsterManager.CloneMonsterState llama a CopyCardState, CopyCharacterStats y
    ///     CopyCharacterAbility, pero NO copia estados: ponerselos al original antes de
    ///     clonar tampoco sirve.
    ///   - El enum TargetMode del juego no tiene ningun "last spawned character" (solo
    ///     LastSpawnedMorsel), asi que no hay modo de objetivo que apunte a la copia.
    ///
    /// Lo unico que queda es reconocerla: CloneMonsterState marca la copia con
    /// SetIsClone(true), y CharacterState.GetIsClone() es publico.
    ///
    /// Es idempotente: si el clon ya tiene el estado, no se lo vuelve a aplicar. Asi los
    /// clones de turnos anteriores que sigan vivos no acumulan cargas cada vez que se
    /// juega la carta.
    ///
    /// NO sobreescribe TestEffect a proposito: cuando el juego comprueba si la carta se
    /// puede jugar todavia no existe ningun clon, asi que cualquier test propio la dejaria
    /// como no jugable.
    ///
    /// Ejemplo (Illusion Twins):
    /// "effects": [
    ///   {
    ///     "id": "IllusionTwinsCopy",
    ///     "name": "CardEffectCopyUnits",
    ///     "target_mode": "drop_target_character",
    ///     "target_team": "monsters",
    ///     "param_int": 1
    ///   },
    ///   {
    ///     "id": "IllusionTwinsFragile",
    ///     "name": "@CardEffectAddStatusEffectToClones",
    ///     "target_mode": "room",
    ///     "target_team": "monsters",
    ///     "param_status_effects": [ { "status": "fragile", "count": 1 } ]
    ///   }
    /// ]
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
