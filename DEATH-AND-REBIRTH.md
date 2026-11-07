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
Whispering Blade now applies Sap 2 to the rear enemy on Accursed; the former Psionic-generating Strike synergy has been removed.

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
<!-- 2026-11-07-1141||codex-succclan-targetless||DEATH-AND-REBIRTH.md||Corregir referencia vigente a antigua sinergia Psionic de Whispering Blade -->

## Endless Shadow: preview bookkeeping fix (2026-11-07)

The native `CardEffectSpawnMonster` applies its -5 HP upgrade to the new character in both modes, but records it on the new card only outside preview. The custom death guard reads card health, so preview previously repeated 10-HP copies instead of 10, then 5, then stopping.

`EndlessShadowPreviewUpgrade` supplies only the missing card bookkeeping. A scope surrounds the native spawner coroutine in preview. A prefix on `CharacterState.ApplyCardUpgrade` accepts only the expected upgrade, a character spawned in preview, a spawner card different from the dying source, and a non-spawn application. It adds the upgrade once to that temporary card before the native HP penalty can process another death. It never applies HP to the unit; native code does that. Inherited upgrades (`fromSpawn`) are excluded. Real combat goes directly through the native spawner. Finally cleanup removes the scope if prediction is cancelled or throws.

The installed spawner is sealed, and neither its JSON nor its public API offers a switch for the preview-only card write. This limited connection preserves native spawning, equipment transfer and state rules; replacing it with Reanimate or Replicate would change the mechanic. No transpiler or duplicate spawner implementation was added.

Regression harness: `D:/Juegos/MT2_mod/salidas/build/succclan-shadow-preview-2026-11-07-1154/Preview.csproj`. It links the actual mod source and models the native boundary: 95 assertions covering HP chains, original-card isolation, inherited/unrelated upgrades, duplicate bookkeeping and cancellation/exception cleanup. This is not a Unity playtest. Check equipment, deaths during OnSpawn and Relentless in game.

<!-- 2026-11-07-1157||codex-succclan-targetless||DEATH-AND-REBIRTH.md||Documentar causa, correccion minima y pruebas de prediccion Endless Shadow -->
