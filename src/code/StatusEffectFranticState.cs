using System.Collections;
using System.Collections.Generic;

using ShinyShoe;

namespace mt2_succclan.Plugin
{
    /// <summary>
    /// Frantic: la unidad enloquece. En su turno NO ataca al enemigo; en su lugar golpea
    /// a un aliado de su propia sala con su propio ataque, y repite el golpe una vez por
    /// cada carga de multistrike. Si no le queda ningun aliado, se golpea a si misma.
    /// Pierde una carga al final de cada turno.
    ///
    /// Port literal del StatusEffectFrantic de SuccClan (Monster Train 1), adaptado al
    /// API de MT2:
    ///   - TestTrigger y OnTriggered reciben ICoreGameManagers (el CombatManager ya no
    ///     viene en inputTriggerParams).
    ///   - ApplyDamageToTargetParameters cambio de campos: vfxAtLoc/showDamageVfx ya no
    ///     existen, ahora es affectedVfx. Ver StatusEffectRecoilState de Conductor.
    ///   - canAttackOrHeal sigue estando en OutputTriggerParams; verificado en el
    ///     ensamblado del juego (MonsterTrain2.Api 2.1.20112166).
    ///
    /// La etapa (on_combat_turn_spark) se declara en json/status_effects/frantic.json.
    /// Esa etapa se invoca dentro de CombatManager.RunUnitTurn, igual que en MT1.
    /// </summary>
    class StatusEffectFranticState : StatusEffectState
    {
        /// <summary>Id del estado multistrike del juego base.</summary>
        private const string MultistrikeStatusId = "multistrike";

        public override bool TestTrigger(
            InputTriggerParams inputTriggerParams,
            OutputTriggerParams outputTriggerParams,
            ICoreGameManagers coreGameManagers)
        {
            var character = inputTriggerParams.associatedCharacter;
            return character != null
                && character.IsAlive
                && character.GetStatusEffectStacks(GetStatusId()) > 0;
        }

        protected override IEnumerator OnTriggered(
            InputTriggerParams inputTriggerParams,
            OutputTriggerParams outputTriggerParams,
            ICoreGameManagers coreGameManagers)
        {
            var thisCharacter = inputTriggerParams.associatedCharacter;
            var combatManager = coreGameManagers.GetCombatManager();

            var spawnPoint = thisCharacter?.GetSpawnPoint(false);
            var room = spawnPoint?.GetRoomOwner();
            if (thisCharacter == null || combatManager == null || room == null)
            {
                // Sin sala no hay a quien golpear, pero el turno igualmente se pierde.
                outputTriggerParams.canAttackOrHeal = false;
                yield break;
            }

            CoreSignals.DamageAppliedPlaySound.Dispatch(Damage.Type.DirectAttack);

            // Una pasada por el golpe normal mas una por cada carga de multistrike.
            int multistrikeStacks = thisCharacter.GetStatusEffectStacks(MultistrikeStatusId);
            for (int i = 0; i <= multistrikeStacks; i++)
            {
                if (!thisCharacter.IsAlive)
                {
                    break;
                }

                var target = FindAllyTarget(combatManager, thisCharacter, room);
                int damageAmount = GetDamageAmount(thisCharacter);

                yield return combatManager.ApplyDamageToTarget(
                    damageAmount,
                    target,
                    new CombatManager.ApplyDamageToTargetParameters
                    {
                        damageType = Damage.Type.DirectAttack,
                        affectedVfx = GetSourceStatusEffectData()?.GetOnAffectedVFX(),
                        relicState = inputTriggerParams.suppressingRelic,
                    });
            }

            // Lo que hace que Frantic sea un debuff de verdad: cancela el ataque normal.
            outputTriggerParams.canAttackOrHeal = false;
        }

        /// <summary>
        /// Primer aliado de la sala, que es el de delante. Puede ser la propia unidad si
        /// es ella la que va al frente, y lo es forzosamente si no le queda ningun aliado.
        /// Es la misma seleccion que hacia el original: "the friendly front unit".
        /// </summary>
        private static CharacterState FindAllyTarget(
            CombatManager combatManager,
            CharacterState thisCharacter,
            RoomState room)
        {
            var charactersInRoom = new List<CharacterState>();
            combatManager.GetAllCharactersInRoom(charactersInRoom, room);

            foreach (var unit in charactersInRoom)
            {
                if (unit != null
                    && unit.IsAlive
                    && unit.GetTeamType() == thisCharacter.GetTeamType())
                {
                    return unit;
                }
            }

            return thisCharacter;
        }

        public override int GetEffectMagnitude(int stacks = 1)
        {
            var character = GetAssociatedCharacter();
            if (character?.GetSpawnPoint(false)?.GetRoomOwner() == null)
            {
                return 0;
            }
            return GetDamageAmount(character);
        }

        /// <summary>El dano es el ataque de la unidad enloquecida, no un valor fijo.</summary>
        private static int GetDamageAmount(CharacterState character)
        {
            return character.GetAttackDamage();
        }
    }
}
