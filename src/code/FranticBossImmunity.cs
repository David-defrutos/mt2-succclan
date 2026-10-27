using HarmonyLib;

namespace mt2_succclan.Plugin
{
    internal static class FranticBossImmunity
    {
        internal const string StatusId = "mt2_succclan.Plugin_frantic";
        internal static bool IsBoss(CharacterState character)
            => character != null && !character.IsDestroyed && (character.IsAnyBoss() || character.IsCompanionBoss());
        internal static bool Blocks(CharacterState character, string statusId)
            => string.Equals(statusId, StatusId, StringComparison.OrdinalIgnoreCase) && (character == null || character.IsDestroyed || IsBoss(character));
    }

    [HarmonyPatch(typeof(CharacterState), nameof(CharacterState.IsImmune))]
    internal static class FranticBossImmunityQueryPatch
    {
        private static void Postfix(CharacterState __instance, string statusEffectId, ref bool __result)
        {
            if (FranticBossImmunity.Blocks(__instance, statusEffectId)) __result = true;
        }
    }

    // Cover the main overload used by spells, units, relics and copied statuses,
    // including effects which explicitly bypass ordinary status immunities.
    [HarmonyPatch(typeof(CharacterState), nameof(CharacterState.AddStatusEffect),
        new Type[] { typeof(string), typeof(int), typeof(CharacterState.AddStatusEffectParams),
            typeof(CharacterState), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(bool) })]
    internal static class FranticBossApplicationPatch
    {
        private static bool Prefix(CharacterState __instance, string statusId)
            => !FranticBossImmunity.Blocks(__instance, statusId);
    }
}
// 2026-10-27-1503||codex-succclan-targetless||src/code/FranticBossImmunity.cs||Evitar consultas de inmunidad Frantic sobre unidades destruidas
