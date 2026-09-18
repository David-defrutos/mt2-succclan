using System.Collections;
using System.Collections.Generic;
using ShinyShoe;

namespace mt2_succclan.Plugin
{
public sealed class CardEffectCopyWithStatuses : CardEffectBase
{
	private int numCopiesOfEachUnit;

	private bool shouldCopyHeroStats;

	public override PropDescriptions CreateEditorInspectorDescriptions()
	{
		return new PropDescriptions
		{
			[CardEffectFieldNames.ParamInt.GetFieldName()] = new PropDescription("Num Copies Of Each Unit"),
			[CardEffectFieldNames.ParamBool.GetFieldName()] = new PropDescription("Copy Hero Stats", "Only applies when copying hero units - copies status effects after spawning a new unit.")
		};
	}

	public override void Setup(CardEffectState cardEffectState)
	{
		base.Setup(cardEffectState);
		numCopiesOfEachUnit = cardEffectState.GetParamInt();
		shouldCopyHeroStats = cardEffectState.GetParamBool();
	}

	public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers)
	{
		return cardEffectParams.targets.Count > 0;
	}

	public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams, ICoreGameManagers coreGameManagers, ISystemManagers sysManagers)
	{
		int numSpawns = 0;
		MonsterManager monsterManager = coreGameManagers.GetMonsterManager();
		HeroManager heroManager = coreGameManagers.GetHeroManager();
		RoomManager roomManager = coreGameManagers.GetRoomManager();
		SaveManager saveManager = coreGameManagers.GetSaveManager();
		CombatManager combatManager = coreGameManagers.GetCombatManager();
		List<CharacterState> originalUnitsInRoom;
		using (GenericPools.GetList(out originalUnitsInRoom))
		{
			RoomState room = roomManager.GetRoom(cardEffectParams.selectedRoom);
			if (room != null)
			{
				room.AddCharactersToList(originalUnitsInRoom, Team.Type.Heroes | Team.Type.Monsters);
			}
			foreach (CharacterState copyUnit in cardEffectParams.targets)
			{
				SpawnPoint spawnPoint = copyUnit.GetSpawnPoint();
				if (spawnPoint == null)
				{
					continue;
				}
				for (int i = 0; i < numCopiesOfEachUnit; i++)
				{
					if (cardEffectState.GetTargetTeamType() == Team.Type.Heroes)
					{
						CharacterState? newHero = null;
						CharacterState? copyStatsFromCharacter = (shouldCopyHeroStats ? copyUnit : null);
						yield return heroManager.SpawnHeroInRoom(copyUnit.GetSourceCharacterData(), cardEffectParams.selectedRoom, newHero, saveManager.PreviewMode, copyStatsFromCharacter);
						if (newHero != null)
						{
							numSpawns++;
						}
						continue;
					}
					CharacterState? createdClone = null;
					yield return monsterManager.CloneMonsterState(copyUnit, cardEffectParams.selectedRoom, delegate(CharacterState newCharacter)
					{
						if (newCharacter != null)
						{
							numSpawns++;
							createdClone = newCharacter;
						}
					}, coreGameManagers, SpawnMode.SelectedSlot, spawnPoint, null, recruitedUnit: false, null, null, null, isCardless: true);
					// CopyCharacterStats runs after the callback and removes states absent
					// from the original. Apply Fragile only once that copying has finished.
					if (createdClone != null && !createdClone.IsDestroyed)
						foreach (var status in cardEffectState.GetSourceCardEffectData().GetParamStatusEffects())
							if (!createdClone.HasStatusEffect(status.statusId))
								createdClone.AddStatusEffect(status.statusId, status.count, allowModification: false);
				}
			}
			if (numSpawns > 0)
			{
				foreach (CharacterState item in originalUnitsInRoom)
				{
					combatManager.QueueTrigger(item, CharacterTriggerData.Trigger.CardMonsterPlayed, null, canAttackOrHeal: true, canFireTriggers: true, null, numSpawns);
				}
			}
			if (numSpawns > 1 && !saveManager.PreviewMode)
			{
				if (cardEffectState.GetTargetTeamType() == Team.Type.Heroes)
				{
					yield return roomManager.HandleRoomUnitOrderPossiblyChanged();
				}
				else
				{
					yield return roomManager.GetRoomUI().CenterCharacters(cardEffectParams.GetSelectedRoom(coreGameManagers.GetRoomManager()));
				}
			}
			monsterManager.RefreshCharacterAbilityUI();
			monsterManager.RefreshEquipmentUI();
		}
	}
}

}
