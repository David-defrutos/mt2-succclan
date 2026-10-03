using System.Collections;

namespace mt2_succclan.Plugin
{
    // Port of CodePointer's MT1 CardEffectSpawnSelfPsionicBlust.
    // Rebirth is a battle-local copy, not a permanent deck upgrade.
    public sealed class CardEffectKnightmareRebirth : CardEffectBase
    {
        private const string Psionic = "mt2_succclan.Plugin_psionic";

        public override PropDescriptions CreateEditorInspectorDescriptions() => new PropDescriptions();

        public override bool TestEffect(CardEffectState effect, CardEffectParams parameters, ICoreGameManagers core)
        {
            var source = parameters.selfTarget;
            var room = parameters.GetSelectedRoom(core.GetRoomManager());
            return source != null && source.IsDead && !source.IsDestroyed && source.GetMaxHP() > 0
                && effect.GetParamInt() > 0 && source.GetStatusEffectStacks(Psionic) >= effect.GetParamInt()
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
            int shifted = room.ShiftSpawnPoints(Team.Type.Monsters, index);
            index = Math.Max(index - shifted, 0);
            if (room.GetRemainingSpawnPointCount(Team.Type.Monsters) <= 0) yield break;
            var point = room.GetMonsterPoint(index);
            if (point == null) yield break;

            int cost = effect.GetParamInt();
            source.RemoveStatusEffect(Psionic, cost, allowModification: false);
            CharacterState? reborn = null;
            // Native cloning preserves the champion's battle buffs, equipment and
            // upgrade triggers. It copies dead HP too; restore full HP immediately
            // after cloning and before the trigger's next effect can run.
            yield return core.GetMonsterManager().CloneMonsterState(source, parameters.selectedRoom,
                character => reborn = character, core, SpawnMode.SelectedSlot, point, isCardless: true);
            if (reborn == null || reborn.IsDestroyed)
            {
                source.AddStatusEffect(Psionic, cost, allowModification: false);
                yield break;
            }
            reborn.SetHealth(reborn.GetMaxHP(), reborn.GetMaxHP());
            var upgrade = new CardUpgradeState();
            upgrade.Setup(effect.GetParamCardUpgradeData());
            yield return reborn.ApplyCardUpgrade(upgrade, fromSpawn: false);
            // Remember additional Multistrike if this copy is copied again later.
            var card = reborn.GetSpawnerCard();
            if (card != null)
                card.GetTemporaryCardStateModifiers().AddUpgrade(upgrade);
            core.GetMonsterManager().RefreshEquipmentUI();
            core.GetMonsterManager().RefreshCharacterAbilityUI();
        }
    }
}
