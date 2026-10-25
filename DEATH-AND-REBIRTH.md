# Death and rebirth fixes — 2026-10-10

## Knightmare: Endless

Current balance: all tiers start at 2 Psionic, increasing by 1 for each
successful rebirth this battle. Each rebirth restores full health, adds
Multistrike 1 and +1/+1, +2/+2 or +3/+3 at tier I/II/III.

The successful-rebirth counter is the number of this tier's growth upgrades
in the spawner card's temporary modifiers. Native cloning carries it to
the next copy. No growth or counter is added to the original deck card,
so both reset next battle. Failed spawns do not advance the cost.

Balance caveat: with Whispering Blade, Multistrike also increases Psionic
production. A cost increment of 1 does not guarantee this combination
cannot sustain itself; combat testing remains necessary.
The native MonsterManager.CloneMonsterState only forwards a selected spawn
point when it is still the source's current point. After compacting the row,
a dying source has no current point, so the old selected-slot call failed.

CardEffectKnightmareRebirth explicitly remembers and detaches the dead
source, including the last slot, compacts the row, uses native free-slot
cloning, then restores the copy's row position. The native clone retains
stats, statuses and equipment. Additional Multistrike is recorded on the
battle copy only. Failed creation refunds Psionic if the source is valid.

Verified after balance change: local build, 175 engine-substitute checks including three tiers,
front/middle/last slot in a full row, position, full health and leftover
Psionic. The substitutes now reproduce the native selected-slot rule that
the previous tests omitted. In-game verification remains pending.

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
