using System.Collections;
using System.Collections.Generic;

namespace mt2_succclan.Plugin
{
    /// <summary>
    /// Como CardEffectCopyUnits, pero aplicando estados a las copias recien creadas.
    ///
    /// Por que hace falta (15-sep-2026): CardEffectCopyUnits.Setup solo lee GetParamInt, o
    /// sea que IGNORA param_upgrade en silencio -- un param_upgrade ahi no da error y no
    /// hace nada. Y tampoco se puede arreglar desde el JSON con un efecto posterior:
    ///   - MonsterManager.CloneMonsterState llama a CopyCardState, CopyCharacterStats y
    ///     CopyCharacterAbility, pero NO copia estados, asi que ponerselos al original
    ///     antes de clonar tampoco sirve.
    ///   - El enum TargetMode del juego no tiene ningun "last spawned character" (solo
    ///     LastSpawnedMorsel), asi que no hay forma de apuntar a la copia despues.
    ///
    /// Solucion: se delega en la clase base y se identifican las copias por diferencia,
    /// comparando quien habia en la sala antes y despues. Vale para cualquier numero de
    /// copias y no depende de ningun modo de objetivo.
    ///
    /// Los estados salen de param_status_effects, con su count como numero de cargas.
    ///
    /// Ejemplo (Illusion Twins):
    /// {
    ///   "id": "IllusionTwinsCopy",
    ///   "name": "@CardEffectCopyUnitsWithStatus",
    ///   "target_mode": "drop_target_character",
    ///   "target_team": "monsters",
    ///   "param_int": 1,
    ///   "param_status_effects": [ { "status": "fragile", "count": 1 } ]
    /// }
    /// </summary>
    public class CardEffectCopyUnitsWithStatus : CardEffectCopyUnits
    {
        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams,
            ICoreGameManagers coreGameManagers,
            ISystemManagers sysManagers)
        {
            if (cardEffectParams.targets.Count == 0)
            {
                yield break;
            }

            RoomState room = cardEffectParams.targets[0].GetCurrentRoom();
            if (room == null)
            {
                yield return base.ApplyEffect(cardEffectState, cardEffectParams, coreGameManagers, sysManagers);
                yield break;
            }

            // Quien habia antes de clonar.
            var antesLista = new List<CharacterState>();
            room.AddCharactersToList(antesLista, Team.Type.Heroes | Team.Type.Monsters);
            var antes = new HashSet<CharacterState>(antesLista);

            yield return base.ApplyEffect(cardEffectState, cardEffectParams, coreGameManagers, sysManagers);

            StatusEffectStackData[] estados = cardEffectState.GetSourceCardEffectData().GetParamStatusEffects();
            if (estados == null || estados.Length == 0)
            {
                yield break;
            }

            // Lo que hay ahora y no habia antes son las copias.
            var despues = new List<CharacterState>();
            room.AddCharactersToList(despues, Team.Type.Heroes | Team.Type.Monsters);

            foreach (CharacterState copia in despues)
            {
                if (copia == null || antes.Contains(copia))
                {
                    continue;
                }

                foreach (StatusEffectStackData estado in estados)
                {
                    if (estado == null || estado.count <= 0)
                    {
                        continue;
                    }

                    copia.AddStatusEffect(
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
    }
}
