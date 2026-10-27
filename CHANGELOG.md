# Changelog

## 0.1.12 — 2026-10-27

### English

Thanks to **Brandon and Rratvar** for reporting bugs and suggesting improvements included in this update.

- **Whispering Blade:** replaced Strike: gain Psionic 1 with Accursed: apply Sap 2 to the rear enemy unit.
- **Knightmare — Endless:** resurrection now retains the same unit and card and restores full health, including health upgrades. Fixed its interaction with Reanimate: existing stacks take priority, then Psionic resurrection becomes available when they run out.
- **Knightmare:** the unit tooltip shows the increasing resurrection cost. Its own death no longer grants Psionic through Harvest, fixing the refund that made the initial Burst 2 effectively cost 1.
- **Frantic:** now causes actual attacks, with Strike, Slay, Spikes, Revenge, Melee Weakness and Lifesteal interactions and native combat prediction. One stack is consumed before attacking; stacks gained during combat remain for a later activation.
- **Abyss Crown:** fixed the extra Psionic stack granted when applying Psionic.
- **Mind Domination / Profane Ascending:** reject invalid boss targets before playing, including companion bosses. Applied the same companion-boss restriction to other affected SuccClan spells.
- **Pain and Pleasure / Arrogance Ghost:** changed Obsessing Spark discard to use Conductor's card-pool filtering.
- **Flare Ribbon:** fixed the Pyre's Armor gain after enemy attacks, including blocked attacks. Clarified that the Pyre gains Armor 4.
- **Mutant Elixirs:** fixed the application of both Mutated and Multistrike 1 to the same unit, removal of the previous mutation and selection of empty rooms.
- **Lust Ghost / Chaos Creation:** clarified their non-boss restriction.
- Removed the full-screen Moonrise effect when generating Obsessing Sparks, including Obsession Vault.
- Disabled obsolete Incubus Butcher animation hooks while using static artwork.
- Added safeguards against accessing destroyed units during targeting, copying and Frantic processing.
- Requires **Trainworks Reloaded 0.7.33** and **Conductor 0.5.15** or newer.

### 简体中文

感谢 **Brandon 和 Rratvar** 报告错误并提出本次更新中的改进建议。

- **低语之刃：**将攻击时获得灵能1改为诅咒：对后排敌方单位施加弱化2。
- **Knightmare — 无尽路线：**复活时保留同一单位及其卡牌，并恢复全部生命值，包括生命强化。修复与复生的交互：优先消耗已有的复生层数，耗尽后可使用灵能复活。
- **Knightmare：**单位提示显示递增的复活消耗。自身死亡不再通过收割获得灵能，修复初始消耗2层实际上因返还而只消耗1层的问题。
- **疯狂：**现在会进行真正的攻击，支持攻击、杀戮、尖刺、复仇、近战易伤和吸血等交互及原生战斗预测。攻击前消耗1层；战斗中获得的层数保留至之后触发。
- **深渊王冠：**修复施加灵能时额外获得1层灵能的效果。
- **支配他人／亵渎升腾：**出牌前拒绝无效的首领目标，包括伴随首领。其他相关SuccClan法术也采用相同的伴随首领限制。
- **喜怒无常／傲慢幽魂：**改用Conductor的卡池筛选来弃掉欲望碎片。
- **薪火缎带：**修复薪火在敌方攻击后获得护甲的效果，包括被完全抵挡的攻击。明确说明薪火获得护甲4。
- **突变药剂：**修复同一单位同时获得变异和多重攻击1、移除之前的变异及选中空房间的问题。
- **色欲幽魂／混沌造物：**明确非首领限制。
- 移除生成欲望碎片时的全屏月升效果，包括痴迷宝库。
- 使用静态美术时，停用Incubus Butcher已废弃的动画补丁。
- 为选取目标、复制和疯狂处理添加保护，避免访问已销毁单位。
- 需要 **Trainworks Reloaded 0.7.33** 和 **Conductor 0.5.15** 或更新版本。

### Español

Gracias a **Brandon y Rratvar** por los errores reportados y las mejoras propuestas que han dado lugar a esta actualización.

- **Whispering Blade:** sustituye Strike: obtiene Psionic 1 por Accursed: aplica Sap 2 al enemigo de la retaguardia.
- **Knightmare — Endless:** la resurrección conserva la misma unidad y carta y recupera toda su vida, incluidas las mejoras de vida. Corregida la interacción con Reanimate: sus cargas tienen prioridad y, al agotarse, puede activarse la resurrección por Psionic.
- **Knightmare:** el tooltip de la unidad muestra el coste creciente de resurrección. Su propia muerte ya no genera Psionic mediante Harvest, corrigiendo el reembolso que hacía que Burst 2 costase efectivamente 1.
- **Frantic:** ahora provoca ataques reales, con interacciones de Strike, Slay, Spikes, Revenge, Melee Weakness y Lifesteal y predicción nativa de combate. Consume una carga antes de atacar; las obtenidas durante el combate quedan para una activación posterior.
- **Abyss Crown:** corregida la carga adicional de Psionic al aplicar Psionic.
- **Mind Domination / Profane Ascending:** rechazan objetivos boss inválidos antes de jugar, incluidos los acompañantes. Aplicada la misma restricción de acompañantes a otros hechizos afectados de SuccClan.
- **Pain and Pleasure / Arrogance Ghost:** el descarte de Obsessing Sparks usa el filtrado por pool de Conductor.
- **Flare Ribbon:** corregida la obtención de armadura de la Pira después de ataques enemigos, incluidos los bloqueados. Aclarado que la Pira obtiene Armor 4.
- **Mutant Elixirs:** corregida la aplicación de Mutated y Multistrike 1 a la misma unidad, la retirada de la mutación anterior y la selección de salas vacías.
- **Lust Ghost / Chaos Creation:** aclarada la restricción de bosses.
- Retirado el efecto lunar de pantalla completa al generar Obsessing Sparks, incluida Obsession Vault.
- Desactivados los parches antiguos de animación de Incubus Butcher mientras usa imagen estática.
- Añadidas protecciones para evitar accesos a unidades destruidas durante la selección de objetivos, las copias y el procesamiento de Frantic.
- Requiere **Trainworks Reloaded 0.7.33** y **Conductor 0.5.15** o posterior.


## 0.1.11 — 2026-10-25

### English

Many thanks to **Brandon and Rratvar** for all the bug reports and improvement suggestions!

#### Balance and cards

- **Knightmare — Abyss:** Psionic Burst now requires and consumes 2 Psionic at all three levels, up from 1. Still draws 1 card and grants 1 ember. There is no per-turn activation limit; infinite setups require more preparation.
- **Knightmare — Endless:** all three levels start at Psionic Burst 2. Each successful resurrection increases the next cost by 1, resetting between battles. Every resurrection grants Multistrike 1 and +1 attack/+1 health, +2/+2 or +3/+3 at levels I, II and III respectively.
- **Ghosts:** removed Obsessing Spark generation on Summon, including Ghost essences. Obsession cooldown reduced from 3 to 2.
- **Shadow Warrior:** starting health increased from 1 to 10; starting attack remains 5. Accursed now grants +2 attack and +1 health permanently. Updated its essence to match the new growth.
- **New unit-shop upgrade — Curseguard Stone:** grants Accursed: gain Armor 5. Available as a common SuccClan upgrade in standard and Soul Savior unit shops.

#### Fixes and clarity

- Benefits paired with Frantic now exclude bosses, including companion bosses. This prevents Abyss Princess, Mind Burning, Dangerous Game and Shadow Lady's Profanity path from strengthening bosses that cannot receive Frantic.
- Applied consistent non-boss targeting to Insanity Reach, Mind Domination and Profane Ascending. Dread Shot still damages friendly units and bosses; only its Frantic effect excludes bosses.
- Normal attack and health buffs now persist through death and resummoning for the rest of the battle. Covers Inception, Knightmare's Abyss and Illusion paths, Shadow Lady, Demon Pioneer, Incubus Butcher, Wrath Ghost, Blood Carnival and Spectral Refuge. Permanent growth remains permanent.
- Removed redundant Consume upgrades and wording when generating Obsessing Sparks; the card already has Consume.
- Updated affected descriptions and upgrade text across all ten supported languages.

#### Presentation

- Refreshed clan logos, card frames, banners and Frantic/Psionic icons.
- Moved CodePointer's original-mod credits and permission acknowledgement to the top of each README language section: English, Simplified Chinese and Spanish.
- Updated feedback links to the dedicated [SuccClan Discord channel](https://discord.com/channels/336546996779483136/1563769828244725770).

### 简体中文

特别感谢 **Brandon 和 Rratvar** 提交的所有错误报告和改进建议！

#### 平衡与卡牌

- **Knightmare — Abyss：**三个等级的 Psionic Burst 均从消耗 1 层 Psionic 改为 2 层。仍抽 1 张牌并获得 1 点 ember，每回合没有触发次数上限；无限循环需要更多准备。
- **Knightmare — Endless：**三个等级均从 Psionic Burst 2 开始。每次成功复活使下次消耗增加 1，战斗之间重置。每次复活获得 Multistrike 1；等级 I、II、III 分别额外获得 +1 攻击/+1 生命、+2/+2、+3/+3。
- **Ghosts：**移除召唤时生成 Obsessing Spark 的效果，包括 Ghost 精华。Obsession 的冷却从 3 降至 2。
- **Shadow Warrior：**初始生命从 1 提高至 10，初始攻击仍为 5。Accursed 现在永久增加 2 攻击和 1 生命。精华也更新为相同的成长效果。
- **新增单位商店升级 — Curseguard Stone：**赋予 Accursed：获得 Armor 5。作为 SuccClan 的普通升级，可在标准模式及 Soul Savior 的单位商店获得。

#### 修复与说明

- 与 Frantic 配套的增益现在排除首领，包括伴随首领。避免 Abyss Princess、Mind Burning、Dangerous Game 及 Shadow Lady 的 Profanity 路线强化无法受到 Frantic 影响的首领。
- Insanity Reach、Mind Domination 和 Profane Ascending 统一排除首领目标。Dread Shot 仍可伤害友方单位和首领；仅其 Frantic 效果排除首领。
- 普通攻击与生命增益现在可在单位死亡并重新召唤后保留至本场战斗结束。涉及 Inception、Knightmare 的 Abyss 和 Illusion 路线、Shadow Lady、Demon Pioneer、Incubus Butcher、Wrath Ghost、Blood Carnival 和 Spectral Refuge。永久成长仍然永久保留。
- 移除生成 Obsessing Spark 时重复添加 Consume 的升级及冗余说明；该卡本身已有 Consume。
- 更新全部十种支持语言中受影响的描述及升级文本。

#### 外观与介绍

- 更新氏族标志、卡框、旗帜以及 Frantic/Psionic 图标。
- 将 CodePointer 的原模组署名及授权说明移至 README 每个语言部分的开头：英语、简体中文和西班牙语。
- 反馈链接更新为专属的 [SuccClan Discord 频道](https://discord.com/channels/336546996779483136/1563769828244725770)。

### Español

¡Muchas gracias a **Brandon y Rratvar** por todos los errores reportados y las mejoras propuestas!

#### Balance y cartas

- **Knightmare — Abyss:** Psionic Burst requiere y consume 2 Psionic en los tres niveles, en lugar de 1. Sigue robando 1 carta y otorgando 1 ember. No hay límite de activaciones por turno; preparar un infinito requiere más trabajo.
- **Knightmare — Endless:** los tres niveles empiezan en Psionic Burst 2. Cada resurrección exitosa aumenta el siguiente coste en 1, que se reinicia entre combates. Cada resurrección otorga Multistrike 1 y +1 de ataque/+1 de vida, +2/+2 o +3/+3 en los niveles I, II y III, respectivamente.
- **Ghosts:** eliminada la generación de Obsessing Spark al invocarse, también en sus esencias. El cooldown de Obsession baja de 3 a 2.
- **Shadow Warrior:** la vida inicial aumenta de 1 a 10; el ataque inicial sigue siendo 5. Accursed otorga ahora +2 de ataque y +1 de vida permanentemente. Su esencia se actualiza con el mismo crecimiento.
- **Nueva mejora de tienda de unidades — Curseguard Stone:** otorga Accursed: obtiene Armor 5. Disponible como mejora común de SuccClan en las tiendas de unidades del modo normal y Soul Savior.

#### Correcciones y claridad

- Las bonificaciones asociadas a Frantic excluyen ahora a los jefes, incluidos los acompañantes. Evita que Abyss Princess, Mind Burning, Dangerous Game y la senda Profanity de Shadow Lady fortalezcan a jefes inmunes a Frantic.
- Insanity Reach, Mind Domination y Profane Ascending excluyen coherentemente a los jefes. Dread Shot conserva el daño a aliados y jefes; solo su efecto de Frantic excluye a los jefes.
- Las mejoras normales de ataque y vida se conservan al morir y volver a invocarse durante el resto del combate. Afecta a Inception, las sendas Abyss e Illusion de Knightmare, Shadow Lady, Demon Pioneer, Incubus Butcher, Wrath Ghost, Blood Carnival y Spectral Refuge. El crecimiento permanente sigue siendo permanente.
- Retiradas las mejoras y menciones redundantes de Consume al generar Obsessing Sparks; la propia carta ya tiene Consume.
- Actualizadas las descripciones y los textos de mejoras afectados en los diez idiomas compatibles.

#### Presentación

- Renovados los logos del clan, marcos, estandartes e iconos de Frantic/Psionic.
- Los créditos del mod original de CodePointer y la mención de su permiso aparecen al principio de cada sección del README: inglés, chino simplificado y español.
- Actualizados los enlaces de comentarios al [canal dedicado de SuccClan en Discord](https://discord.com/channels/336546996779483136/1563769828244725770).


## 0.1.8 — 2026-10-10

- **Knightmare, Endless:** fixed failed respawns caused by native selected-slot cloning. Frees the dying champion's slot explicitly, including the last slot, and restores its row position after cloning.

- **Endless Shadow:** stop the death-triggered summon when the next copy would have no health. Prevents the zero-health sacrifice/respawn chain; clarified the tooltip in all 10 languages.
- Added more Incubus Butcher animation poses: 18 active attack poses, 8 hit reactions and 12 death poses, with a stable neutral pose.
- Removed the abrupt opening poses from Incubus Butcher's attack, using its stable neutral pose before lifting the axe.
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
<!-- 2026-10-25-1630||codex-succclan-targetless||CHANGELOG.md||Documentar puntos 2–6 aprobados; motor de robo/ember Abyss sin limitar -->
<!-- 2026-10-25-1701||codex-succclan-targetless||CHANGELOG.md||Registrar Burst 2 aprobado en los tres niveles de Abyss, sin límite de activaciones -->
<!-- 2026-10-25-1706||codex-succclan-targetless||CHANGELOG.md||Consolidar Unreleased: Abyss Burst 2, Endless progresivo, Ghosts, Shadow Warrior, tienda, filtros de bosses, persistencia y presentación -->
<!-- 2026-10-25-1836||codex-succclan-targetless||CHANGELOG.md||Publicación 0.1.9: 13 cambios en EN/ZH-CN/ES y agradecimiento a Brandon y Rratvar -->
<!-- 2026-10-25-1839||codex-succclan-targetless||CHANGELOG.md||Añadir canal dedicado Discord a 0.1.9 en tres idiomas; agradecimientos Brandon/Rratvar siguen específicos de versión -->
<!-- 2026-10-25-1916||codex-succclan-targetless||CHANGELOG.md||Curseguard Stone: Armor 2 -> 5 en changelog 0.1.9 EN/ZH-CN/ES -->
<!-- 2026-10-25-1917||codex-succclan-targetless||CHANGELOG.md||Encabezar 0.1.10 conservando íntegro el changelog trilingüe, Armor 5 y agradecimientos -->
<!-- 2026-10-25-1950||codex-succclan-targetless||CHANGELOG.md||Retirar únicamente anuncio de nuevas animaciones Butcher de 0.1.10 en EN/ZH-CN/ES; conservar demás cambios e historial -->
<!-- 2026-10-25-1952||codex-succclan-targetless||CHANGELOG.md||Encabezar 0.1.11 conservando changelog trilingüe sin anuncio de animaciones Butcher -->

<!-- 2026-10-27-1506||codex-succclan-targetless||CHANGELOG.md||Añadir notas Unreleased EN/ZH/ES de bugs corregidos y límites de validación; conservar historial -->

<!-- 2026-10-27-1554||codex-succclan-targetless||CHANGELOG.md||Ampliar Unreleased EN/ZH/ES con descarte de biblioteca, validación nativa, inmunidad Pyre, Mutant Elixirs y dependencias -->

<!-- 2026-10-27-2046||codex-succclan-targetless||CHANGELOG.md||Añadir cambio aprobado de Whispering Blade en Unreleased EN/ZH/ES -->

<!-- 2026-10-27-2048||codex-succclan-targetless||CHANGELOG.md||Retirar datos sin cambios de Whispering Blade y Pain/Arrogance en Unreleased EN/ZH/ES; describir solo cambios -->

<!-- 2026-10-27-2050||codex-succclan-targetless||CHANGELOG.md||Retirar mención a diez idiomas en entradas Lust Ghost/Chaos Creation de Unreleased EN/ZH/ES -->

<!-- 2026-10-27-2052||codex-succclan-targetless||CHANGELOG.md||Preparar 0.1.12 EN/ZH/ES: solo cambios del producto, sin valores intactos ni idiomas/procesos/pruebas -->

<!-- 2026-10-27-2055||codex-succclan-targetless||CHANGELOG.md||Precisar agradecimiento específico a Brandon/Rratvar por reportes y mejoras de 0.1.12 en EN/ZH/ES -->
