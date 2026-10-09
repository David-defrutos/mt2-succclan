# Changelog

## 0.1.8 — 2026-10-10

- **Knightmare, Endless:** fixed failed respawns caused by native selected-slot cloning. Frees the dying champion's slot explicitly, including the last slot, and restores its row position after cloning.

- **Endless Shadow:** stop the death-triggered summon when the next copy would have no health. Prevents the zero-health sacrifice/respawn chain; clarified the tooltip in all 10 languages.
- Added more Incubus Butcher animation poses: 20 attack poses, 8 hit reactions and 12 death poses, with a stable neutral pose.
- Made Incubus Butcher attack and hit reactions easier to see: longer key-pose holds, normal visual playback speed, and deferred return to idle so clips can finish. Combat damage and callbacks retain their native timing.

- Replaced Incubus Butcher's stepped idle poses with subtle, continuous breathing on one stable pose, eliminating the idle frame size jumps.

## 0.1.7 — 2026-10-07

- Fixed Incubus Butcher animated combat artwork sizing to match sprite dimensions; used the static character shader for frame texture compatibility.

## 0.1.6 — 2026-10-07

- Added Incubus Butcher combat animations: idle, attack, hit reaction and death.

- Changed Succubus Torturer Accursed to apply Melee Weakness 1 to the front enemy unit, replacing Advance. Strike is unchanged. Updated all ten supported languages.

## 0.1.5 — 2026-10-06

- Added magical visual effects and sound feedback to units, spells, rooms and equipment, including Psionic and Frantic.
- Animated the card artwork of Knightmare, Shadow Lady and all ten rare cards with subtle motion and magical glows. Combat character artwork is unchanged.
- Fixed missing Psionic Burst tooltips on all Knightmare upgrade paths.
- Fixed generated Obsessing Spark tooltips to show Consume and Reserve: your Pyre takes 1 damage.
- Registered Obsessing Sparks for simultaneous Reserve resolution at the end of the turn.
- Succubus Torturer now has Accursed: Advance the rear enemy unit. This moves the last enemy to the front without dealing damage.
- Matched Succubus Torturer's Strike wording to Railbeater and corrected its English name. Updated text in all ten supported languages.
- Updated the minimum Trainworks Reloaded dependency to 0.7.30.

This is still an ALPHA. Please report bugs and balance feedback through GitHub issues or the Monster Train Discord modding channel.

## 0.1.4 — 2026-10-04

- Fixed SuccClan not appearing after installation through r2modman or Thunderstore Mod Manager: DLL, JSON and textures are now packaged together under plugins/.
- Added explicit missing-content diagnostics and a count of configured JSON files.

## 0.1.3 — 2026-10-03

- Replaced README language links with plain directions in English, Chinese and Spanish.
- Replaced the Chinese screenshot anchor link with plain directions.
- No changes to card stats, effects or translations.

## 0.1.2 — 2026-10-03

- Separated the README language navigation into one link per line.
- Corrected the Discord link label to "Monster Train Discord's Mod Channel".
- No changes to card stats, effects or translations.

## 0.1.1 — 2026-10-03

- Added complete SuccClan text coverage for all ten languages supported by Monster Train 2.
- Added Spanish, French, German, Brazilian Portuguese, Russian, Traditional Chinese, Japanese and Korean translations; retained existing English and Simplified Chinese text.
- Added a Simplified Chinese section to the README, ordered English, Chinese, Spanish, with language links at the top.
- Added GitHub issues and Monster Train Discord modding links for bug reports, balance feedback and translation corrections.
- No card stats or gameplay effects changed in this update.

New translations are a first pass and have not been reviewed by native speakers or checked in every language in game.

## 0.1.0

- First playable ALPHA release of the MT2 port, with CodePointer's permission.
