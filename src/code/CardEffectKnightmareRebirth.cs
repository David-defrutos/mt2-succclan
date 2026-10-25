using System.Collections;

namespace mt2_succclan.Plugin
{
    // Adapted from CodePointer's MT1 CardEffectSpawnSelfPsionicBlust.
    // Rebirth is a battle-local copy, not a permanent deck upgrade.
    public sealed class CardEffectKnightmareRebirth : CardEffectBase
    {
        private const string Psionic = "mt2_succclan.Plugin_psionic";

        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();

        internal static int GetRebirthCost(CardEffectState effect, CharacterState source)
        {
            int cost = effect.GetParamInt();
            var data = effect.GetParamCardUpgradeData();
            var card = source.GetSpawnerCard();
            if (data == null || card == null) return cost;
            // Each successful rebirth adds exactly one upgrade to the cloned
            // card's battle-only modifiers. Native card cloning carries these
            // forward; the original deck card remains untouched for next battle.
            foreach (var upgrade in card.GetTemporaryCardStateModifiers().GetCardUpgrades())
                if (upgrade.GetCardUpgradeDataId() == data.GetID()) cost++;
            return cost;
        }

        public override bool TestEffect(CardEffectState effect, CardEffectParams parameters, ICoreGameManagers core)
        {
            var source = parameters.selfTarget;
            var room = parameters.GetSelectedRoom(core.GetRoomManager());
            return source != null && source.IsDead && !source.IsDestroyed && source.GetMaxHP() > 0
                && effect.GetParamInt() > 0 && source.GetStatusEffectStacks(Psionic) >= GetRebirthCost(effect, source)
                && room != null && room.IsRoomEnabled();
        }

        public override IEnumerator ApplyEffect(CardEffectState effect, CardEffectParams parameters,
            ICoreGameManagers core, ISystemManagers systems)
        {
            if (!TestEffect(effect, parameters, core)) yield break;
            var source = parameters.selfTarget;
            var room = parameters.GetSelectedRoom(core.GetRoomManager());
            if (source == null || room == null) yield break;
            var oldPoint = source.GetSpawnPoint(true);
            if (oldPoint == null) yield break;

            int index = oldPoint.GetIndexInRoom();
            // ShiftSpawnPoints does not clear the last slot. Detach explicitly,
            // retaining the old room for the remaining death/harvest callbacks.
            source.SetLastKnownSpawnPoint();
            source.RemoveFromSpawnPoint();
            int shifted = room.ShiftSpawnPoints(Team.Type.Monsters, index);
            index = Math.Max(index - shifted, 0);
            if (room.GetRemainingSpawnPointCount(Team.Type.Monsters) <= 0) yield break;

            int cost = GetRebirthCost(effect, source);
            source.RemoveStatusEffect(Psionic, cost, allowModification: false);
            CharacterState? reborn = null;
            // Native cloning preserves the champion's battle buffs, equipment and
            // upgrade triggers. It copies dead HP too; restore full HP immediately
            // after cloning and before the trigger's next effect can run.
            yield return core.GetMonsterManager().CloneMonsterState(source, parameters.selectedRoom,
                character => reborn = character, core, SpawnMode.FrontSlot, isCardless: true);
            // MT2 CloneMonsterState only forwards a selected slot when it is
            // still the source's current slot; a detached dead source has none.
            // Spawn in a native free slot, then restore the original row order.
            if (reborn == null || reborn.IsDestroyed)
            {
                if (!source.IsDestroyed)
                    source.AddStatusEffect(Psionic, cost, allowModification: false);
                Plugin.Logger.LogWarning("Knightmare Endless: clone failed; Psionic refunded when source remains valid.");
                yield break;
            }
            reborn.SetHealth(reborn.GetMaxHP(), reborn.GetMaxHP());
            var rebornPoint = reborn.GetSpawnPoint(false);
            if (rebornPoint != null && rebornPoint.GetIndexInRoom() != index)
                room.RearrangeCharacter(Team.Type.Monsters, rebornPoint.GetIndexInRoom(), index);
            var upgrade = new CardUpgradeState();
            upgrade.Setup(effect.GetParamCardUpgradeData());
            yield return reborn.ApplyCardUpgrade(upgrade, fromSpawn: false);
            // Remember battle-local growth and use each upgrade as a rebirth counter.
            var card = reborn.GetSpawnerCard();
            if (card != null)
                card.GetTemporaryCardStateModifiers().AddUpgrade(upgrade);
            reborn.SetHealth(reborn.GetMaxHP(), reborn.GetMaxHP());
            Plugin.Logger.LogInfo($"Knightmare Endless: respawned at full health; spent {cost} Psionic, remaining {reborn.GetStatusEffectStacks(Psionic)}, next cost {GetRebirthCost(effect, reborn)}.");
            core.GetMonsterManager().RefreshEquipmentUI();
            core.GetMonsterManager().RefreshCharacterAbilityUI();
        }
    }
}
