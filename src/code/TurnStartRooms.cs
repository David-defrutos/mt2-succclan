using System.Collections;
using HarmonyLib;

namespace mt2_succclan.Plugin
{
    public sealed class RoomStateSuccClanTurnStartEffects : RoomStateModifierBase
    {
        private readonly List<CardEffectState> effects = new();
        private int lastAppliedTurn = -1;
        public override void Initialize(RoomModifierData data, SaveManager save)
        {
            base.Initialize(data, save);
            effects.Clear();
            lastAppliedTurn = -1;
            foreach (var effect in data.GetParamCardEffectData())
            {
                var state = new CardEffectState();
                state.Setup(effect, null);
                effects.Add(state);
            }
        }
        public IEnumerator Apply(RoomState room, ICoreGameManagers core)
        {
            int turn = core.GetCombatManager().GetTurnCount();
            if (lastAppliedTurn == turn) yield break;
            lastAppliedTurn = turn;
            yield return ShowTriggeredVFX(room, core);
            yield return core.GetCombatManager().ApplyEffects(effects, room.GetRoomIndex());
        }
    }

    // DrawOpeningHand delegates to DrawHand, so both ordinary turn draw paths
    // resolve once here, after drawing and before the player's input is enabled.
    [HarmonyPatch(typeof(CardManager), nameof(CardManager.DrawHand))]
    internal static class SuccClanTurnStartRoomPatch
    {
        private static void Postfix(AllGameManagers ___allGameManagers, ref IEnumerator __result)
            => __result = AfterDraw(__result, ___allGameManagers);

        private static IEnumerator AfterDraw(IEnumerator original, AllGameManagers managers)
        {
            yield return original;
            if (managers == null) yield break;
            var core = managers.GetCoreManagers();
            if (core.GetCombatManager().GetCombatPhase() != CombatManager.Phase.MonsterTurn) yield break;
            var rooms = core.GetRoomManager();
            for (int i = 0; i < rooms.GetNumRooms(); i++)
            {
                var room = rooms.GetRoom(i);
                if (room == null) continue;
                foreach (var modifier in room.GetRoomStateModifiersFromTrainRoomAttachments<RoomStateSuccClanTurnStartEffects>(Team.Type.Monsters).ToList())
                    yield return modifier.Apply(room, core);
            }
        }
    }
}
