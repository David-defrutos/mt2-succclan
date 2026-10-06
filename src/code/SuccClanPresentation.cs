using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace mt2_succclan.Plugin
{
    /// <summary>Presentation only: reuse installed MT2 resources and native cues.</summary>
    internal static class SuccClanPresentation
    {
        private static readonly HashSet<string> ReportedErrors = new(StringComparer.Ordinal);

        internal static bool CanPresent(ICoreGameManagers core)
        {
            return core != null && !core.GetSaveManager().PreviewMode
                && AllGameManagers.Instance != null
                && !AllGameManagers.Instance.GetScreenManager().GetScreenActive(ScreenName.Compendium);
        }

        private static bool IsClanCard(CardState? card)
        {
            return card?.GetCardDataID()?.StartsWith(MyPluginInfo.PLUGIN_GUID + "-Card-", StringComparison.Ordinal) == true;
        }

        private static bool IsClanCharacter(CharacterState? character)
        {
            return character != null && !character.IsDestroyed
                && character.GetSourceCharacterData()?.GetID()?.StartsWith(
                    MyPluginInfo.PLUGIN_GUID + "-Character-", StringComparison.Ordinal) == true;
        }

        internal static bool IsClanContext(CardEffectState state, CardEffectParams parameters)
        {
            return IsClanCard(parameters.playedCard) || IsClanCard(state.GetParentCardState())
                || IsClanCharacter(parameters.selfTarget)
                || IsClanCharacter(parameters.characterThatActivatedAbility);
        }

        internal static void PlayEffect(CardEffectState state, CardEffectParams parameters, ICoreGameManagers core)
        {
            if (!CanPresent(core)) return;
            var managers = AllGameManagers.Instance;
            if (managers == null) return;
            try
            {
                var room = core.GetRoomManager().GetRoom(parameters.selectedRoom);
                // The same native method used by card UI. appliedVfxId prevents a
                // damage class from drawing a second copy of the same impact.
                state.PlayEffectVfx(parameters, core, room,
                    managers.GetGameVfxManager(), false);
            }
            catch (Exception error)
            {
                // A missing visual resource must not cancel the gameplay effect.
                if (ReportedErrors.Add(error.GetType().FullName ?? error.GetType().Name))
                    Plugin.Logger.LogWarning("SuccClan presentation failed; gameplay continues: " + error.Message);
            }
        }

        internal static void PlayEffectSound(CardEffectState state, CardEffectParams parameters, ICoreGameManagers core)
        {
            if (!CanPresent(core) || !IsClanContext(state, parameters)) return;
            // Heal, buff, status, spawn and hand sounds are already supplied by
            // their native operations. Only fill the missing spell attack/move.
            var effect = state.GetCardEffect();
            if ((effect is CardEffectDamage || effect is CardEffectDamagePerSourceAttack)
                && parameters.targets.Count > 0 && parameters.selfTarget == null
                && state.GetTargetMode() != TargetMode.Pyre)
                SoundManager.PlaySfxSignal.Dispatch("Combat_Attack");
            else if (effect is CardEffectBump && parameters.targets.Count > 0)
                SoundManager.PlaySfxSignal.Dispatch("Combat_Ascend");
        }

        internal static CharacterState.MovementDoneToken BeginFranticAttack(CharacterState character, ICoreGameManagers core)
        {
            var timing = core.GetCombatManager().ActiveTiming;
            character.PlayCharacterSound("Combat_AttackPrep");
            // Keep the allied/self impact on the actual victim. Suppress forward
            // attack particles and projectiles which would point at the enemy.
            return character.DoMovementAttacking(timing.UnitAttackWindUpDuration,
                timing.UnitAttackDuration, CharacterState.MovementActionType.Attack,
                suppressAttackVfx: true);
        }

        internal static void ShowSpawn(CharacterState character)
        {
            var managers = AllGameManagers.Instance;
            if (managers == null || !IsClanCharacter(character) || character.PreviewMode
                || character.SpawnedInPreviewMode || !character.IsAlive
                || managers.GetScreenManager().GetScreenActive(ScreenName.Compendium)) return;
            try
            {
                var visual = character.GetSourceCharacterData().GetDeathVfx();
                var ui = character.GetCharacterUI();
                if (visual != null && ui != null)
                    ui.ShowEffectVFX(character, visual);
            }
            catch (Exception error)
            {
                if (ReportedErrors.Add("Spawn:" + error.GetType().FullName))
                    Plugin.Logger.LogWarning("SuccClan spawn visual failed; gameplay continues: " + error.Message);
            }
        }
    }

    /// <summary>Triggers and rooms can apply effects without the card UI callback.</summary>
    [HarmonyPatch(typeof(GameEffectHelper), nameof(GameEffectHelper.ApplyEffect))]
    internal static class SuccClanEffectPresentationPatch
    {
        [HarmonyPrefix]
        private static void BeforeApply(ref CombatManager.ApplyPreEffectsVfxAction onPreEffectsFiredVfx,
            ICoreGameManagers coreGameManagers)
        {
            if (!SuccClanPresentation.CanPresent(coreGameManagers)) return;
            var original = onPreEffectsFiredVfx;
            onPreEffectsFiredVfx = (state, parameters) =>
            {
                // Target collection happens inside ApplyEffect, before this
                // callback. Inspect context here, never before targets exist.
                original?.Invoke(state, parameters);
                if (!SuccClanPresentation.IsClanContext(state, parameters)) return;
                if (original == null)
                    SuccClanPresentation.PlayEffect(state, parameters, coreGameManagers);
                SuccClanPresentation.PlayEffectSound(state, parameters, coreGameManagers);
            };
        }
    }

    [HarmonyPatch(typeof(CharacterState), nameof(CharacterState.Setup))]
    internal static class SuccClanSpawnPresentationPatch
    {
        [HarmonyPostfix]
        private static IEnumerator AfterSetup(IEnumerator __result, CharacterState __instance)
        {
            yield return __result;
            SuccClanPresentation.ShowSpawn(__instance);
        }
    }
}
