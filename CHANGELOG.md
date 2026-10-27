# Changelog

## Unreleased — bug fixes under validation

### English

Thanks to **Brandon and Rratvar** for the reports and suggestions.

- **Whispering Blade:** retains +5 Attack, 1 Ember and uncommon rarity. Replace Strike: gain Psionic 1 with Accursed: apply Sap 2 to the rear enemy unit.

- **Pain and Pleasure / Arrogance Ghost:** use Conductor's pool-filtered discard, preserving the requirement and quantity of 2 Obsessing Sparks. Pain and Pleasure still draws 3 cards.
- **Mutant Elixirs:** grant both Mutated and Multistrike 1 to the same unit, remove the previous mutation correctly and avoid choosing empty rooms.
- Use native target validation for Mind Domination and Profane Ascending. Disable the four archived Butcher animation patches while its artwork is static. Requires Trainworks Reloaded 0.7.33 and Conductor 0.5.15 or newer.

- **Knightmare — Endless:** replaced cardless cloning with native Reanimate on the same unit and card. Restore full upgraded health before death triggers resolve; existing Reanimate stacks have priority. Psionic resurrection becomes available after those stacks are exhausted.
- **Knightmare:** show the increasing resurrection cost in the unit tooltip. Its own death no longer generates Psionic through Harvest, preventing a refund that made the initial Burst 2 behave like Burst 1.
- **Frantic:** redirect native attacks to the front unit on the attacker's own team, retaining native attack interactions and combat preview. Consume one stack before attacking; stacks gained during combat remain available for a later activation.
- **Abyss Crown:** use the native effect that adds an extra Psionic stack to positive applications, rather than modifying the status magnitude.
- **Mind Domination:** validate non-boss targets before playing, including companion bosses. Complete the same validation boundary for other SuccClan spells with this restriction.
- **Flare Ribbon:** grant Armor 4 to the Pyre after an enemy attack resolves, bypassing its status immunity as native Pyre-targeted status effects do. In-game verification is still required.
- **Lust Ghost / Chaos Creation:** clarify their non-boss restriction in all ten languages. Remove the full-screen Moonrise effect from Spark generation, including Obsession Vault.
- Guard targeting, cloning and Frantic immunity against destroyed characters. The reported Envy Ghost assertion still needs an in-game reproduction to confirm its full cause.

### 简体中文

感谢 **Brandon 和 Rratvar** 的问题报告和建议。

- **低语之刃：**保留攻击力+5、费用1和非普通稀有度。将攻击时获得灵能1改为诅咒：对后排敌方单位施加弱化2。

- **喜怒无常／傲慢幽魂：**改用Conductor按卡池筛选的弃牌效果，保留2张欲望碎片的要求和数量。喜怒无常仍抽3张牌。
- **突变药剂：**使同一单位同时获得变异和多重攻击1，正确移除之前的变异，并避免选中空房间。
- 支配他人和亵渎升腾改用原生目标验证。屠夫使用静态美术时停用四个已归档的动画补丁。需要Trainworks Reloaded 0.7.33和Conductor 0.5.15或更新版本。

- **Knightmare — 无尽路线：**使用原生复生流程替代无卡牌复制，保留同一单位及其卡牌。在死亡触发结算前恢复强化后的全部生命值；已有的复生层数优先消耗，耗尽后才能使用灵能复活。
- **Knightmare：**单位提示显示递增的复活消耗。自身死亡不再通过收割获得灵能，避免初始消耗2层实际上返还1层。
- **疯狂：**将原生攻击改为攻击己方最前方单位，保留攻击交互和战斗预览。攻击前消耗1层；战斗中获得的层数保留至之后触发。
- **深渊王冠：**改用原生的额外施加层数效果，使每次正向施加灵能额外获得1层，而非修改状态强度。
- **支配他人：**出牌前检查非首领目标，包括伴随首领。补全其他具有该限制的SuccClan法术的同一检查环节。
- **薪火缎带：**在敌方攻击结算后使薪火获得4点护甲，并像原生薪火状态效果一样绕过其状态免疫。仍需实战验证。
- **色欲幽魂／混沌造物：**在全部十种语言中明确非首领限制。移除生成火花时的全屏月升效果，包括痴迷宝库。
- 防止选取目标、复制和疯狂免疫逻辑访问已销毁单位。嫉妒幽魂的报错仍需实战复现以确认完整原因。

### Español

Gracias a **Brandon y Rratvar** por los reportes y propuestas.

- **Whispering Blade:** mantiene +5 de ataque, coste 1 e infrecuente. Sustituye Strike: obtiene Psionic 1 por Accursed: aplica Sap 2 al enemigo de la retaguardia.

- **Pain and Pleasure / Arrogance Ghost:** usan el descarte filtrado por pool de Conductor, conservando el requisito y cantidad de 2 Obsessing Sparks. Pain and Pleasure sigue robando 3 cartas.
- **Mutant Elixirs:** concede Mutated y Multistrike 1 a la misma unidad, retira correctamente la mutación anterior y evita seleccionar salas vacías.
- Validación nativa de objetivos para Mind Domination y Profane Ascending. Desactivados los cuatro parches archivados de animación del Butcher mientras usa arte estático. Requiere Trainworks Reloaded 0.7.33 y Conductor 0.5.15 o posterior.

- **Knightmare — Endless:** sustituida la copia sin carta por Reanimate nativo sobre la misma unidad y carta. Recupera toda la vida máxima mejorada antes de resolver los triggers de muerte; las cargas de Reanimate existentes tienen prioridad. La resurrección por Psionic queda disponible al agotarlas.
- **Knightmare:** coste creciente de resurrección en el tooltip de la unidad. Su propia muerte ya no genera Psionic mediante Harvest, evitando el reembolso que hacía que Burst 2 se comportase como Burst 1.
- **Frantic:** redirige el ataque nativo a la unidad delantera del propio bando, conservando las interacciones de ataque y la predicción de combate. Consume una carga antes de atacar; las obtenidas durante el combate quedan para una activación posterior.
- **Abyss Crown:** usa el efecto nativo que añade una carga adicional al aplicar Psionic positivo, en lugar de modificar su magnitud.
- **Mind Domination:** valida objetivos que no sean bosses antes de jugarla, incluidos los acompañantes. Completada la misma validación para otros hechizos SuccClan con esta restricción.
- **Flare Ribbon:** concede Armor 4 a la Pira tras resolver el ataque enemigo, evitando su inmunidad a estados como hacen los efectos nativos dirigidos a la Pira. Pendiente de comprobación en partida.
- **Lust Ghost / Chaos Creation:** excepción de bosses explícita en los diez idiomas. Retirado Moonrise de pantalla completa al generar Sparks, incluida Obsession Vault.
- Protecciones frente a personajes destruidos en selección, clonación e inmunidad Frantic. La causa completa del error de Envy Ghost sigue pendiente de reproducir en partida.


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
