using System.Collections;
using System.Collections.Generic;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    // Keep the native RunUnitTurn: only redirect its attack targets. Native combat
    // owns Strike, Slay, Revenge, Spikes, Lifesteal, Multistrike and preview.
    class StatusEffectFranticState : StatusEffectState
    {
        public override bool TestTrigger(InputTriggerParams input, OutputTriggerParams output,
            ICoreGameManagers core)
        {
            var unit = input.associatedCharacter;
            return unit != null && !unit.IsDestroyed && unit.IsAlive && output.canAttackOrHeal
                && !FranticBossImmunity.IsBoss(unit) && unit.GetStatusEffectStacks(GetStatusId()) > 0;
        }

        protected override IEnumerator OnTriggered(InputTriggerParams input, OutputTriggerParams output,
            ICoreGameManagers core)
        {
            var unit = input.associatedCharacter;
            if (unit == null || unit.IsDestroyed || !unit.IsAlive) yield break;
            if (!FranticNativeTurn.Activate(unit))
            {
                // No native attack context: never accidentally attack the enemy.
                output.canAttackOrHeal = false;
                yield break;
            }
            // Consume before combat. The native hit may kill/destroy the attacker.
            unit.RemoveStatusEffect(GetStatusId(), 1, allowModification: false);
        }

        public override int GetEffectMagnitude(int stacks = 1)
        {
            var unit = GetAssociatedCharacter();
            return unit == null || unit.IsDestroyed ? 0 : unit.GetAttackDamage();
        }
    }

    internal static class FranticNativeTurn
    {
        internal sealed class Scope
        {
            internal CharacterState Unit = null!;
            internal bool Activated;
        }
        private static readonly List<Scope> Scopes = new();

        internal static bool Activate(CharacterState unit)
        {
            for (int i = Scopes.Count - 1; i >= 0; i--)
                if (Scopes[i].Unit == unit) { Scopes[i].Activated = true; return true; }
            return false;
        }

        internal static bool IsActive(CharacterState unit)
        {
            for (int i = Scopes.Count - 1; i >= 0; i--)
                if (Scopes[i].Unit == unit) return Scopes[i].Activated;
            return false;
        }

        internal static IEnumerator Run(IEnumerator original, CharacterState unit)
        {
            if (unit == null || unit.IsDestroyed) yield break;
            var scope = new Scope { Unit = unit };
            Scopes.Add(scope);
            try { yield return original; }
            finally { Scopes.Remove(scope); }
        }
    }

    [HarmonyPatch(typeof(CombatManager), nameof(CombatManager.RunUnitTurn))]
    internal static class FranticNativeTurnPatch
    {
        [HarmonyPostfix]
        private static void After(CharacterState attackerState, ref IEnumerator __result)
            => __result = FranticNativeTurn.Run(__result, attackerState);
    }

    [HarmonyPatch(typeof(CombatManager), "CollectTargetDataInRunAttack")]
    internal static class FranticNativeTargetsPatch
    {
        [HarmonyPrefix]
        private static void Before(CharacterState attackerState, ref ICharacterManager targetsManager,
            ref List<CharacterState> overrideTargets)
        {
            if (attackerState == null || attackerState.IsDestroyed || !FranticNativeTurn.IsActive(attackerState)) return;
            targetsManager = attackerState.GetCharacterManager();
            var allies = new List<CharacterState>();
            attackerState.GetSpawnPoint(false)?.GetRoomOwner()?.AddCharactersToList(allies, attackerState.GetTeamType());
            var front = allies.FirstOrDefault(unit => unit != null && !unit.IsDestroyed && unit.IsAlive);
            overrideTargets = new List<CharacterState>();
            if (front != null) overrideTargets.Add(front);
            else if (attackerState.IsAlive) overrideTargets.Add(attackerState);
        }

        [HarmonyPostfix]
        private static void After(CharacterState attackerState, ref TargetMode attackTargetMode,
            ref TargetHelper.CollectTargetsData __result)
        {
            if (attackerState == null || attackerState.IsDestroyed || !FranticNativeTurn.IsActive(attackerState)) return;
            // Frantic hits the front unit even if the attacker normally has Sweep.
            attackTargetMode = TargetMode.FrontInRoom;
            __result.targetMode = attackTargetMode;
        }
    }
}
// 2026-10-27-1456||codex-succclan-targetless||src/code/StatusEffectFranticState.cs||Frantic redirige ataques nativos al frente aliado; consume antes del golpe y conserva triggers y preview
