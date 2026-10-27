<!-- Documento generado el 2026-10-27-1506 -->

# Death and rebirth fixes — 2026-10-27

## Knightmare: Endless — native rebirth, 2026-10-27

All tiers start at 2 Psionic. Each successful Psionic rebirth raises the next
cost by 1 and grants Multistrike 1 plus +1/+1, +2/+2 or +3/+3 by tier.

The former cardless CloneMonsterState implementation is retired. The JSON
Extinguish effect is now a marker for localized descriptions and tooltips;
it does not spawn a new character or fire a second resurrection.

A narrowly scoped CheckDeathCardTriggers coroutine adapter finds that marker,
checks silence/mute, room availability, death, and the current Psionic cost.
Existing native Undying (Reanimate) has priority and spends no Psionic or
growth. When no native stacks remain and Psionic is sufficient, the adapter
supplies one unmodified Undying stack and spends the required Psionic before
Harvest can resolve. It then delegates to the original death routine.

A ReviveFromUndyingStatus adapter observes only these prepared revivals.
After native HP=1 and death-flag reset, it applies the tier's existing growth
upgrade and restores full new maximum HP, before native Harvest and queued
OnReanimated/OnDeath events. Native animation, card identity, equipment,
statuses, row position and death bookkeeping are preserved.

Cost is based on the character's applied growth upgrades, including native
preview copies. Real revivals also record growth in the spawner card's
battle-only modifiers, matching the lifetime handling of the native
CardEffectAddCardUpgradeToUnits. Preview never writes to real card modifiers.
The unit tooltip resolves [effect0.power] against that same character count.

Knightmare's own death is excluded from its Harvest effect, so it cannot
refund its own cost. Harvest from other units remains native. This is a
small subclass of CardEffectAddStatusEffect because the installed trigger
filters have no simple exclude-own-dying-character option.

Native JSON alone cannot implement a threshold-dependent, growing cost on
Reanimate with full-HP restoration; unconditional Undying would resurrect
without sufficient Psionic, while native cardless spawning loses identity.
Only this missing connection is custom; combat/death operations remain native.

Validation: 241 assertions using engine substitutes and metadata contract
checks against the installed MT2 DLL. These do not replace Unity playtests.
Test full HP, all tiers, cost 2/3/4, save/reload, preview, equipment, Rage,
Regeneration, Corruption, native Reanimate exhaustion, Recall and Reform.
The attack-generation synergy with Whispering Blade still needs balance tests.

## Endless Shadow

The native spawner applies the -5 HP upgrade after summoning. A zero-health
copy is immediately sacrificed and can re-enter death/spawn resolution.
CardEffectEndlessShadowRebirth guards the native spawner: it only summons
when the card's spawn health is greater than 5. Standard sequence:
15 -> 10 -> 5 -> stop. Ten language descriptions clarify this condition.

The native spawner copies card upgrades and transfers equipment, except
return-to-hand equipment. Combat-only statuses such as Rage, Regen and
Corruption are not copied. Equipment is transferred after the HP penalty;
ordinary equipped HP does not extend the final guarded respawn.

Verified: local build and 16 engine-substitute checks. The reported null
reference was in CheckForDeath; its exact null operand was not captured.
The zero-health recursion path is fixed; in-game confirmation remains pending.

<!-- 2026-10-27-1506||codex-succclan-targetless||DEATH-AND-REBIRTH.md||Sustituir explicación vigente de clonación por puente Reanimate nativo, prioridad y checklist de partida -->
