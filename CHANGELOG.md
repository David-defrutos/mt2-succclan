# Changelog

[English changelog](#english) · [Español](#spanish) · [Français](#french) · [Deutsch](#german) · [Русский](#russian) · [Português (Brasil)](#portuguese-brazil) · [简体中文](#chinese) · [繁體中文](#chinese-traditional) · [한국어](#korean) · [日本語](#japanese)

Latest release in all ten game languages. Full history in English and Spanish.

## English

### 0.1.13 — 2026-10-27 — ALPHA

- Reorganized the changelog by language, added navigation links and expanded its translations and Spanish history.

### 0.1.12 — 2026-10-27 — ALPHA

Thanks to **Brandon and Rratvar** for reporting bugs and suggesting improvements included in this update.

- **Whispering Blade:** replaced Strike: gain Psionic 1 with Accursed: apply Sap 2 to the rear enemy unit.
- **Knightmare — Endless:** resurrection now retains the same unit and card and restores full health, including health upgrades. Fixed its interaction with Reanimate: existing stacks take priority, then Psionic resurrection becomes available when they run out.
- **Knightmare:** the unit tooltip shows the increasing resurrection cost. Its own death no longer grants Psionic through Harvest, fixing the refund that made the initial Burst 2 effectively cost 1.
- **Frantic:** now causes actual attacks, with Strike, Slay, Spikes, Revenge, Melee Weakness and Lifesteal interactions and native combat prediction. One stack is consumed when its first attack starts; turns without an attack retain the stack. Clarified that the front unit can attack itself.
- **Abyss Crown:** fixed the extra Psionic stack granted when applying Psionic and corrected singular wording.
- **Mind Domination / Profane Ascending:** reject invalid boss targets before playing, including companion bosses. Applied the same companion-boss restriction to other affected SuccClan spells.
- **Profane Ascending:** can only target enemy units, matching its description.
- **Pain and Pleasure / Arrogance Ghost:** changed Obsessing Spark discard to use Conductor's card-pool filtering.
- **Flare Ribbon:** fixed the Pyre's Armor gain after enemy attacks, including blocked attacks. Clarified that the Pyre gains Armor 4.
- **Mutant Elixirs:** fixed the application of both Mutated and Multistrike 1 to the same unit, removal of the previous mutation and selection of empty rooms.
- **Lust Ghost / Chaos Creation:** clarified their non-boss restriction.
- Removed the full-screen Moonrise effect when generating Obsessing Sparks, including Obsession Vault.
- Disabled obsolete Incubus Butcher animation hooks while using static artwork.
- Added safeguards against accessing destroyed units during targeting, copying and Frantic processing.
- Requires **Trainworks Reloaded 0.7.33** and **Conductor 0.5.15** or newer.

### 0.1.11 — 2026-10-25

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

### 0.1.8 — 2026-10-10

- **Knightmare, Endless:** fixed failed respawns caused by native selected-slot cloning. Frees the dying champion's slot explicitly, including the last slot, and restores its row position after cloning.

- **Endless Shadow:** stop the death-triggered summon when the next copy would have no health. Prevents the zero-health sacrifice/respawn chain; clarified the tooltip in all 10 languages.
- Added more Incubus Butcher animation poses: 18 active attack poses, 8 hit reactions and 12 death poses, with a stable neutral pose.
- Removed the abrupt opening poses from Incubus Butcher's attack, using its stable neutral pose before lifting the axe.
- Made Incubus Butcher attack and hit reactions easier to see: longer key-pose holds, normal visual playback speed, and deferred return to idle so clips can finish. Combat damage and callbacks retain their native timing.

- Replaced Incubus Butcher's stepped idle poses with subtle, continuous breathing on one stable pose, eliminating the idle frame size jumps.

### 0.1.7 — 2026-10-07

- Fixed Incubus Butcher animated combat artwork sizing to match sprite dimensions; used the static character shader for frame texture compatibility.

### 0.1.6 — 2026-10-07

- Added Incubus Butcher combat animations: idle, attack, hit reaction and death.

- Changed Succubus Torturer Accursed to apply Melee Weakness 1 to the front enemy unit, replacing Advance. Strike is unchanged. Updated all ten supported languages.

### 0.1.5 — 2026-10-06

- Added magical visual effects and sound feedback to units, spells, rooms and equipment, including Psionic and Frantic.
- Animated the card artwork of Knightmare, Shadow Lady and all ten rare cards with subtle motion and magical glows. Combat character artwork is unchanged.
- Fixed missing Psionic Burst tooltips on all Knightmare upgrade paths.
- Fixed generated Obsessing Spark tooltips to show Consume and Reserve: your Pyre takes 1 damage.
- Registered Obsessing Sparks for simultaneous Reserve resolution at the end of the turn.
- Succubus Torturer now has Accursed: Advance the rear enemy unit. This moves the last enemy to the front without dealing damage.
- Matched Succubus Torturer's Strike wording to Railbeater and corrected its English name. Updated text in all ten supported languages.
- Updated the minimum Trainworks Reloaded dependency to 0.7.30.

This is still an ALPHA. Please report bugs and balance feedback through GitHub issues or the Monster Train Discord modding channel.

### 0.1.4 — 2026-10-04

- Fixed SuccClan not appearing after installation through r2modman or Thunderstore Mod Manager: DLL, JSON and textures are now packaged together under plugins/.
- Added explicit missing-content diagnostics and a count of configured JSON files.

### 0.1.3 — 2026-10-03

- Replaced README language links with plain directions in English, Chinese and Spanish.
- Replaced the Chinese screenshot anchor link with plain directions.
- No changes to card stats, effects or translations.

### 0.1.2 — 2026-10-03

- Separated the README language navigation into one link per line.
- Corrected the Discord link label to "Monster Train Discord's Mod Channel".
- No changes to card stats, effects or translations.

### 0.1.1 — 2026-10-03

- Added complete SuccClan text coverage for all ten languages supported by Monster Train 2.
- Added Spanish, French, German, Brazilian Portuguese, Russian, Traditional Chinese, Japanese and Korean translations; retained existing English and Simplified Chinese text.
- Added a Simplified Chinese section to the README, ordered English, Chinese, Spanish, with language links at the top.
- Added GitHub issues and Monster Train Discord modding links for bug reports, balance feedback and translation corrections.
- No card stats or gameplay effects changed in this update.

New translations are a first pass and have not been reviewed by native speakers or checked in every language in game.

### 0.1.0

- First playable ALPHA release of the MT2 port, with CodePointer's permission.

## Spanish

### 0.1.13 — 2026-10-27 — ALPHA

- Changelog reorganizado por idioma, con enlaces de navegación, traducciones ampliadas e historial español completado.

### 0.1.12 — 2026-10-27 — ALPHA

Gracias a **Brandon y Rratvar** por los errores reportados y las mejoras propuestas que han dado lugar a esta actualización.

- **Whispering Blade:** sustituye Strike: obtiene Psionic 1 por Accursed: aplica Sap 2 al enemigo de la retaguardia.
- **Knightmare — Endless:** la resurrección conserva la misma unidad y carta y recupera toda su vida, incluidas las mejoras de vida. Corregida la interacción con Reanimate: sus cargas tienen prioridad y, al agotarse, puede activarse la resurrección por Psionic.
- **Knightmare:** el tooltip de la unidad muestra el coste creciente de resurrección. Su propia muerte ya no genera Psionic mediante Harvest, corrigiendo el reembolso que hacía que Burst 2 costase efectivamente 1.
- **Frantic:** ahora provoca ataques reales, con interacciones de Strike, Slay, Spikes, Revenge, Melee Weakness y Lifesteal y predicción nativa de combate. Consume una carga al comenzar su primer ataque; si no ataca, conserva la carga. Aclarado que la unidad delantera puede atacarse a sí misma.
- **Abyss Crown:** corregida la carga adicional de Psionic al aplicar Psionic y los textos en singular.
- **Mind Domination / Profane Ascending:** rechazan objetivos boss inválidos antes de jugar, incluidos los acompañantes. Aplicada la misma restricción de acompañantes a otros hechizos afectados de SuccClan.
- **Profane Ascending:** solo puede seleccionar unidades enemigas, como indica su descripción.
- **Pain and Pleasure / Arrogance Ghost:** el descarte de Obsessing Sparks usa el filtrado por pool de Conductor.
- **Flare Ribbon:** corregida la obtención de armadura de la Pira después de ataques enemigos, incluidos los bloqueados. Aclarado que la Pira obtiene Armor 4.
- **Mutant Elixirs:** corregida la aplicación de Mutated y Multistrike 1 a la misma unidad, la retirada de la mutación anterior y la selección de salas vacías.
- **Lust Ghost / Chaos Creation:** aclarada la restricción de bosses.
- Retirado el efecto lunar de pantalla completa al generar Obsessing Sparks, incluida Obsession Vault.
- Desactivados los parches antiguos de animación de Incubus Butcher mientras usa imagen estática.
- Añadidas protecciones para evitar accesos a unidades destruidas durante la selección de objetivos, las copias y el procesamiento de Frantic.
- Requiere **Trainworks Reloaded 0.7.33** y **Conductor 0.5.15** o posterior.

### 0.1.11 — 2026-10-25

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

### 0.1.8 — 2026-10-10

- **Knightmare, Endless:** corregidas las reapariciones fallidas por la selección nativa del hueco al clonar. Se libera explícitamente el hueco del campeón muerto, incluida la última posición, y se restaura su posición en la fila.
- **Endless Shadow:** la invocación al morir se detiene si la siguiente copia no tendría vida. Evita la cadena de sacrificio y reaparición con vida cero; aclarado el tooltip.
- Añadidas poses de animación de Incubus Butcher: 18 de ataque, 8 de reacción a golpes y 12 de muerte, con una pose neutra estable.
- Retiradas las poses iniciales bruscas del ataque de Incubus Butcher; usa su pose neutra antes de levantar el hacha.
- Ataques y reacciones a golpes más legibles: poses clave más largas, reproducción visual normal y retorno a reposo aplazado para terminar el clip. Daño y callbacks conservan su momento nativo.
- Sustituidas las poses de reposo escalonadas por respiración continua sobre una pose estable, eliminando los saltos de tamaño.

### 0.1.7 — 2026-10-07

- Corregido el tamaño del arte animado en combate de Incubus Butcher para respetar las dimensiones del sprite; usa el shader del personaje estático para compatibilidad de texturas.

### 0.1.6 — 2026-10-07

- Añadidas animaciones de combate de Incubus Butcher: reposo, ataque, reacción a golpes y muerte.
- Accursed de Succubus Torturer aplica Melee Weakness 1 al enemigo delantero en lugar de Advance.

### 0.1.5 — 2026-10-06

- Añadidos efectos visuales mágicos y sonidos a unidades, hechizos, salas y equipos, incluidos Psionic y Frantic.
- Animado el arte de las cartas de Knightmare, Shadow Lady y las diez cartas raras con movimientos y brillos sutiles.
- Corregidos los tooltips de Psionic Burst ausentes en las sendas de Knightmare.
- Corregido el tooltip de Obsessing Spark generado para mostrar Consume y Reserve: la Pira recibe 1 de daño.
- Registrada la resolución simultánea de Reserve para Obsessing Spark al final del turno.
- Succubus Torturer obtiene Accursed: Advance del enemigo de la retaguardia, que lo mueve al frente sin causar daño.
- Texto de Strike de Succubus Torturer alineado con Railbeater y nombre inglés corregido.
- Dependencia mínima de Trainworks Reloaded actualizada a 0.7.30.

Sigue siendo una ALPHA. Comunica errores y comentarios de balance en GitHub o en el canal de modding de Monster Train Discord.

### 0.1.4 — 2026-10-04

- Corregido que SuccClan no apareciera al instalarlo con r2modman o Thunderstore Mod Manager: DLL, JSON y texturas se empaquetan juntos bajo plugins/.
- Añadidos diagnósticos de contenido ausente y un recuento de JSON configurados.

### 0.1.3 — 2026-10-03

- Sustituidos los enlaces de idioma del README por indicaciones de lectura en inglés, chino y español.
- Sustituido el enlace chino a las capturas por indicaciones de lectura.

### 0.1.2 — 2026-10-03

- Navegación por idiomas del README separada en un enlace por línea.
- Corregida la etiqueta del enlace de Discord a «Monster Train Discord's Mod Channel».

### 0.1.1 — 2026-10-03

- Añadidas traducciones de los textos del clan a los idiomas de Monster Train 2 que faltaban.
- Añadida una sección en chino simplificado al README, después del inglés y antes del español, con enlaces de idioma al principio.
- Añadidos enlaces a GitHub y al canal de modding de Monster Train Discord para errores, balance y correcciones de traducción.

Las traducciones nuevas son una primera versión y aún no habían sido revisadas por hablantes nativos ni comprobadas en partida en todos los idiomas.

### 0.1.0

- Primera ALPHA jugable del port a MT2, con permiso de CodePointer.

## French

### 0.1.13 — 2026-10-27 — ALPHA

- Changelog réorganisé par langue, avec liens de navigation, traductions enrichies et historique espagnol complété.

### 0.1.12 — 2026-10-27 — ALPHA

Merci à **Brandon et Rratvar** pour les bugs signalés et les améliorations proposées dans cette mise à jour.

- **Whispering Blade :** remplace Strike : gagne Psionic 1 par Accursed : applique Sap 2 à l’unité ennemie à l’arrière.
- **Knightmare — Endless :** la résurrection conserve la même unité et sa carte et restaure toute sa santé, améliorations comprises. Les charges de Reanimate sont utilisées en priorité ; une fois épuisées, la résurrection par Psionic peut se déclencher.
- **Knightmare :** l’infobulle affiche le coût croissant de résurrection. Sa propre mort ne donne plus de Psionic via Harvest, supprimant le remboursement qui réduisait effectivement Burst 2 à 1.
- **Frantic :** provoque de vraies attaques avec les interactions Strike, Slay, Spikes, Revenge, Melee Weakness, Lifesteal et la prédiction native. Consomme une charge au début de la première attaque ; sans attaque, la charge est conservée. Précise que l’unité de tête peut s’attaquer elle-même.
- **Abyss Crown :** corrige la charge supplémentaire de Psionic et les formulations au singulier.
- **Mind Domination / Profane Ascending :** refusent les cibles boss invalides avant de jouer, y compris les boss compagnons. Les autres sorts concernés appliquent aussi cette restriction aux compagnons.
- **Profane Ascending :** ne peut cibler que des unités ennemies, conformément à sa description.
- **Pain and Pleasure / Arrogance Ghost :** utilisent le filtrage par pool de Conductor pour défausser Obsessing Spark.
- **Flare Ribbon :** corrige le gain d’armure du Brasier après les attaques ennemies, même bloquées. Précise que le Brasier gagne Armor 4.
- **Mutant Elixirs :** corrige l’application de Mutated et Multistrike 1 à la même unité, le retrait de la mutation précédente et la sélection de salles vides.
- **Lust Ghost / Chaos Creation :** précise la restriction excluant les boss.
- Supprime l’effet Moonrise plein écran lors de la génération d’Obsessing Spark, y compris avec Obsession Vault.
- Désactive les anciens correctifs d’animation d’Incubus Butcher lorsque son illustration est statique.
- Ajoute des protections contre l’accès aux unités détruites pendant le ciblage, la copie et Frantic.
- Nécessite **Trainworks Reloaded 0.7.33** et **Conductor 0.5.15** ou versions ultérieures.

## German

### 0.1.13 — 2026-10-27 — ALPHA

- Changelog nach Sprache gegliedert, Navigationslinks hinzugefügt sowie Übersetzungen und spanischen Verlauf erweitert.

### 0.1.12 — 2026-10-27 — ALPHA

Danke an **Brandon und Rratvar** für die Fehlerberichte und Verbesserungsvorschläge zu diesem Update.

- **Whispering Blade:** Strike: Psionic 1 erhalten wird durch Accursed: Sap 2 auf die hinterste gegnerische Einheit ersetzt.
- **Knightmare — Endless:** Die Wiederbelebung erhält dieselbe Einheit und Karte und stellt die gesamte Gesundheit einschließlich Verbesserungen wieder her. Vorhandene Reanimate-Stapel haben Vorrang; danach kann die Psionic-Wiederbelebung auslösen.
- **Knightmare:** Der Tooltip zeigt die steigenden Wiederbelebungskosten. Der eigene Tod gewährt durch Harvest kein Psionic mehr; die Rückerstattung, die Burst 2 effektiv auf 1 reduzierte, entfällt.
- **Frantic:** Führt echte Angriffe mit Strike, Slay, Spikes, Revenge, Melee Weakness, Lifesteal und nativer Kampfvorhersage aus. Verbraucht einen Stapel beim ersten Angriff; ohne Angriff bleibt er erhalten. Der Text erklärt, dass die vorderste Einheit sich selbst angreifen kann.
- **Abyss Crown:** Zusätzlichen Psionic-Stapel und Formulierungen im Singular korrigiert.
- **Mind Domination / Profane Ascending:** Ungültige Bossziele einschließlich Begleitbosse werden vor dem Ausspielen abgelehnt. Andere betroffene Zauber schließen Begleitbosse ebenfalls aus.
- **Profane Ascending:** Kann entsprechend der Beschreibung nur gegnerische Einheiten als Ziel wählen.
- **Pain and Pleasure / Arrogance Ghost:** Verwenden Conductors Kartenpool-Filter zum Abwerfen von Obsessing Spark.
- **Flare Ribbon:** Rüstungsgewinn der Höllenglut nach gegnerischen Angriffen korrigiert, auch bei blockierten Treffern. Der Text nennt die Höllenglut als Empfänger von Armor 4.
- **Mutant Elixirs:** Mutated und Multistrike 1 werden derselben Einheit verliehen; das Entfernen der vorherigen Mutation und die Auswahl leerer Räume wurden korrigiert.
- **Lust Ghost / Chaos Creation:** Bossausschluss im Text präzisiert.
- Vollbild-Moonrise beim Erzeugen von Obsessing Spark entfernt, einschließlich Obsession Vault.
- Veraltete Animations-Patches für Incubus Butcher bei statischer Illustration deaktiviert.
- Zugriffe auf zerstörte Einheiten bei Zielwahl, Kopieren und Frantic abgesichert.
- Benötigt **Trainworks Reloaded 0.7.33** und **Conductor 0.5.15** oder neuer.

## Russian

### 0.1.13 — 2026-10-27 — ALPHA

- Список изменений сгруппирован по языкам; добавлены ссылки навигации, расширены переводы и дополнена испанская история версий.

### 0.1.12 — 2026-10-27 — ALPHA

Спасибо **Brandon и Rratvar** за сообщения об ошибках и предложения, вошедшие в это обновление.

- **Whispering Blade:** Strike: получить Psionic 1 заменено на Accursed: наложить Sap 2 на заднего врага.
- **Knightmare — Endless:** воскрешение сохраняет того же бойца и карту и восстанавливает всё здоровье с учётом улучшений. Сначала расходуются заряды Reanimate; затем доступно воскрешение за Psionic.
- **Knightmare:** подсказка показывает растущую стоимость воскрешения. Собственная смерть больше не даёт Psionic через Harvest, устраняя возврат, фактически снижавший Burst 2 до 1.
- **Frantic:** вызывает настоящие атаки с взаимодействиями Strike, Slay, Spikes, Revenge, Melee Weakness, Lifesteal и штатным прогнозом боя. Один заряд расходуется при начале первой атаки; без атаки он сохраняется. Уточнено, что передний боец может атаковать себя.
- **Abyss Crown:** исправлены дополнительный заряд Psionic и формулировки в единственном числе.
- **Mind Domination / Profane Ascending:** до розыгрыша отклоняют недопустимые цели-боссов, включая сопровождающих боссов. То же ограничение для сопровождающих действует у других затронутых заклинаний.
- **Profane Ascending:** выбирает только врагов, как указано в описании.
- **Pain and Pleasure / Arrogance Ghost:** сброс Obsessing Spark использует фильтр карточного пула Conductor.
- **Flare Ribbon:** исправлено получение брони Пеклом после атак врага, включая заблокированные. Уточнено, что Armor 4 получает Пекло.
- **Mutant Elixirs:** исправлены выдача Mutated и Multistrike 1 одному бойцу, снятие предыдущей мутации и выбор пустых комнат.
- **Lust Ghost / Chaos Creation:** уточнено исключение боссов.
- Убран полноэкранный эффект Moonrise при создании Obsessing Spark, включая Obsession Vault.
- Отключены устаревшие патчи анимации Incubus Butcher при использовании статичного изображения.
- Добавлена защита от обращений к уничтоженным бойцам при выборе целей, копировании и обработке Frantic.
- Требуются **Trainworks Reloaded 0.7.33** и **Conductor 0.5.15** или новее.

## Portuguese Brazil

### 0.1.13 — 2026-10-27 — ALPHA

- Changelog reorganizado por idioma, com links de navegação, traduções ampliadas e histórico em espanhol completado.

### 0.1.12 — 2026-10-27 — ALPHA

Obrigado a **Brandon e Rratvar** pelos relatos de erros e sugestões de melhorias incluídas nesta atualização.

- **Whispering Blade:** substitui Strike: ganha Psionic 1 por Accursed: aplica Sap 2 à unidade inimiga na retaguarda.
- **Knightmare — Endless:** a ressurreição mantém a mesma unidade e carta e restaura toda a vida, incluindo melhorias. Os acúmulos de Reanimate têm prioridade; após se esgotarem, a ressurreição por Psionic fica disponível.
- **Knightmare:** a dica da unidade mostra o custo crescente de ressurreição. A própria morte não concede mais Psionic por Harvest, corrigindo o reembolso que fazia Burst 2 custar efetivamente 1.
- **Frantic:** provoca ataques reais com interações de Strike, Slay, Spikes, Revenge, Melee Weakness, Lifesteal e previsão nativa. Consome um acúmulo ao iniciar o primeiro ataque; se não atacar, conserva o acúmulo. Esclarece que a unidade da frente pode atacar a si mesma.
- **Abyss Crown:** corrigidos o acúmulo adicional de Psionic e os textos no singular.
- **Mind Domination / Profane Ascending:** rejeitam alvos chefes inválidos antes da jogada, incluindo chefes acompanhantes. Outros feitiços afetados também excluem os acompanhantes.
- **Profane Ascending:** só pode selecionar unidades inimigas, conforme a descrição.
- **Pain and Pleasure / Arrogance Ghost:** o descarte de Obsessing Spark usa o filtro de grupo de cartas do Conductor.
- **Flare Ribbon:** corrigido o ganho de armadura da Pira após ataques inimigos, inclusive bloqueados. Esclarece que a Pira recebe Armor 4.
- **Mutant Elixirs:** corrigidas a aplicação de Mutated e Multistrike 1 à mesma unidade, a remoção da mutação anterior e a seleção de salas vazias.
- **Lust Ghost / Chaos Creation:** esclarecida a restrição de chefes.
- Removido o efeito Moonrise de tela inteira ao gerar Obsessing Spark, incluindo Obsession Vault.
- Desativados os antigos patches de animação de Incubus Butcher ao usar imagem estática.
- Adicionadas proteções contra acesso a unidades destruídas na seleção de alvos, cópias e processamento de Frantic.
- Requer **Trainworks Reloaded 0.7.33** e **Conductor 0.5.15** ou mais recente.

## Chinese

### 0.1.13 — 2026-10-27 — ALPHA

- 更新日志按语言分组，添加导航链接，扩充翻译并补全西班牙语版本历史。

### 0.1.12 — 2026-10-27 — ALPHA

感谢 **Brandon 和 Rratvar** 报告错误并提出本次更新中的改进建议。

- **低语之刃：**将攻击时获得灵能1改为诅咒：对后排敌方单位施加弱化2。
- **Knightmare — 无尽路线：**复活时保留同一单位及其卡牌，并恢复全部生命值，包括生命强化。修复与复生的交互：优先消耗已有的复生层数，耗尽后可使用灵能复活。
- **Knightmare：**单位提示显示递增的复活消耗。自身死亡不再通过收割获得灵能，修复初始消耗2层实际上因返还而只消耗1层的问题。
- **疯狂：**现在会进行真正的攻击，支持攻击、杀戮、尖刺、复仇、近战易伤和吸血等交互及原生战斗预测。首次攻击开始时消耗1层；没有进行攻击的回合保留层数。明确说明前排单位可能攻击自身。
- **深渊王冠：**修复施加灵能时额外获得1层灵能的效果，并修正单数措辞。
- **支配他人／亵渎升腾：**出牌前拒绝无效的首领目标，包括伴随首领。其他相关SuccClan法术也采用相同的伴随首领限制。
- **亵渎晋升：**仅能选择敌方单位，与卡牌描述一致。
- **喜怒无常／傲慢幽魂：**改用Conductor的卡池筛选来弃掉欲望碎片。
- **薪火缎带：**修复薪火在敌方攻击后获得护甲的效果，包括被完全抵挡的攻击。明确说明薪火获得护甲4。
- **突变药剂：**修复同一单位同时获得变异和多重攻击1、移除之前的变异及选中空房间的问题。
- **色欲幽魂／混沌造物：**明确非首领限制。
- 移除生成欲望碎片时的全屏月升效果，包括痴迷宝库。
- 使用静态美术时，停用Incubus Butcher已废弃的动画补丁。
- 为选取目标、复制和疯狂处理添加保护，避免访问已销毁单位。
- 需要 **Trainworks Reloaded 0.7.33** 和 **Conductor 0.5.15** 或更新版本。

### 0.1.11 — 2026-10-25

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

## Chinese Traditional

### 0.1.13 — 2026-10-27 — ALPHA

- 更新日誌依語言分組，加入導覽連結，擴充翻譯並補齊西班牙語版本歷史。

### 0.1.12 — 2026-10-27 — ALPHA

感謝 **Brandon 和 Rratvar** 回報錯誤並提出本次更新中的改進建議。

- **Whispering Blade：**將 Strike：獲得 Psionic 1 改為 Accursed：對後排敵方單位施加 Sap 2。
- **Knightmare — Endless：**復活時保留同一單位及其卡牌，恢復包含生命強化的全部生命。已有的 Reanimate 層數優先消耗，耗盡後可使用 Psionic 復活。
- **Knightmare：**單位提示顯示遞增的復活消耗。自身死亡不再透過 Harvest 獲得 Psionic，修正初始 Burst 2 因返還而實際只消耗1層的問題。
- **Frantic：**現在會進行真正的攻擊，支援 Strike、Slay、Spikes、Revenge、Melee Weakness、Lifesteal 互動及原生戰鬥預測。首次攻擊開始時消耗1層；沒有攻擊的回合保留層數。明確說明前排單位可能攻擊自身。
- **Abyss Crown：**修正施加 Psionic 時額外獲得1層的效果及單數措辭。
- **Mind Domination / Profane Ascending：**出牌前拒絕無效的首領目標，包括伴隨首領。其他相關法術也採用相同的伴隨首領限制。
- **Profane Ascending：**僅能選擇敵方單位，與描述一致。
- **Pain and Pleasure / Arrogance Ghost：**棄置 Obsessing Spark 改用 Conductor 的卡池篩選。
- **Flare Ribbon：**修正薪火在敵方攻擊後獲得護甲的效果，包括被完全抵擋的攻擊。明確說明薪火獲得 Armor 4。
- **Mutant Elixirs：**修正同一單位同時獲得 Mutated 和 Multistrike 1、移除先前變異及選中空房間的問題。
- **Lust Ghost / Chaos Creation：**明確非首領限制。
- 移除生成 Obsessing Spark 時的全螢幕 Moonrise 效果，包括 Obsession Vault。
- 使用靜態美術時停用 Incubus Butcher 已廢棄的動畫補丁。
- 為選取目標、複製和 Frantic 處理加入保護，避免存取已銷毀單位。
- 需要 **Trainworks Reloaded 0.7.33** 和 **Conductor 0.5.15** 或更新版本。

## Korean

### 0.1.13 — 2026-10-27 — ALPHA

- 변경 내역을 언어별로 정리하고 탐색 링크를 추가했으며, 번역과 스페인어 버전 기록을 보완했습니다.

### 0.1.12 — 2026-10-27 — ALPHA

이번 업데이트의 버그를 제보하고 개선을 제안해 주신 **Brandon과 Rratvar**에게 감사드립니다.

- **Whispering Blade:** Strike: Psionic 1 획득을 Accursed: 후열 적 유닛에게 Sap 2 부여로 변경했습니다.
- **Knightmare — Endless:** 부활 시 같은 유닛과 카드를 유지하고 강화된 최대 체력을 모두 회복합니다. 기존 Reanimate 중첩을 먼저 사용하고, 소진한 뒤 Psionic 부활을 사용할 수 있습니다.
- **Knightmare:** 유닛 툴팁에 증가하는 부활 비용을 표시합니다. 자신의 죽음으로 Harvest의 Psionic을 얻지 않도록 수정하여 Burst 2가 사실상 1만 소모하던 문제를 해결했습니다.
- **Frantic:** 실제 공격을 수행하며 Strike, Slay, Spikes, Revenge, Melee Weakness, Lifesteal 및 기본 전투 예측과 상호작용합니다. 첫 공격이 시작될 때 중첩 1개를 소모하며, 공격하지 않으면 유지합니다. 전열 유닛이 자신을 공격할 수 있음을 명시했습니다.
- **Abyss Crown:** Psionic 부여 시 추가 중첩과 단수 표현을 수정했습니다.
- **Mind Domination / Profane Ascending:** 동반 보스를 포함한 유효하지 않은 보스 대상을 사용 전에 거부합니다. 다른 관련 주문에도 동일한 동반 보스 제한을 적용했습니다.
- **Profane Ascending:** 설명대로 적 유닛만 대상으로 지정할 수 있습니다.
- **Pain and Pleasure / Arrogance Ghost:** Obsessing Spark 버리기에 Conductor의 카드 풀 필터를 사용합니다.
- **Flare Ribbon:** 막힌 공격을 포함하여 적의 공격 후 화로가 방어도를 얻도록 수정했습니다. 화로가 Armor 4를 얻는다는 점을 명확히 했습니다.
- **Mutant Elixirs:** 같은 유닛에 Mutated와 Multistrike 1을 함께 부여하고 이전 변이를 제거하며 빈 객실을 선택하지 않도록 수정했습니다.
- **Lust Ghost / Chaos Creation:** 보스 제외 조건을 명확히 했습니다.
- Obsession Vault를 포함하여 Obsessing Spark 생성 시 전체 화면 Moonrise 효과를 제거했습니다.
- 정적 이미지를 사용하는 Incubus Butcher의 오래된 애니메이션 패치를 비활성화했습니다.
- 대상 지정, 복제, Frantic 처리에서 파괴된 유닛에 접근하지 않도록 보호했습니다.
- **Trainworks Reloaded 0.7.33** 및 **Conductor 0.5.15** 이상이 필요합니다.

## Japanese

### 0.1.13 — 2026-10-27 — ALPHA

- 変更履歴を言語別に整理し、移動リンクを追加しました。翻訳を拡充し、スペイン語の過去の履歴を補完しました。

### 0.1.12 — 2026-10-27 — ALPHA

不具合の報告と今回の改善案を寄せてくださった **Brandon と Rratvar** に感謝します。

- **Whispering Blade：**Strike：Psionic 1獲得を、Accursed：最後列の敵ユニットにSap 2付与に変更しました。
- **Knightmare — Endless：**復活時に同じユニットとカードを維持し、強化分を含む体力を全回復します。既存のReanimateを優先して消費し、なくなるとPsionicによる復活が可能になります。
- **Knightmare：**ユニットのツールチップに増加する復活コストを表示します。自身の死亡ではHarvestのPsionicを得なくなり、Burst 2が実質1消費になっていた問題を修正しました。
- **Frantic：**実際の攻撃を行い、Strike、Slay、Spikes、Revenge、Melee Weakness、Lifestealや標準の戦闘予測に対応します。最初の攻撃開始時に1スタック消費し、攻撃しないターンは維持します。最前列のユニットが自分自身を攻撃する場合があることを明記しました。
- **Abyss Crown：**Psionic付与時の追加スタックと単数表現を修正しました。
- **Mind Domination / Profane Ascending：**同伴ボスを含む無効なボス対象を使用前に拒否します。他の対象となる呪文にも同伴ボスの制限を適用しました。
- **Profane Ascending：**説明どおり敵ユニットのみを対象にできます。
- **Pain and Pleasure / Arrogance Ghost：**Obsessing Sparkの破棄にConductorのカードプールフィルターを使用します。
- **Flare Ribbon：**防がれた攻撃も含め、敵の攻撃後にパイアがアーマーを得るよう修正しました。パイアがArmor 4を得ることを明確にしました。
- **Mutant Elixirs：**同じユニットへのMutatedとMultistrike 1の付与、前の変異の解除、空の部屋の選択を修正しました。
- **Lust Ghost / Chaos Creation：**ボスを対象外とする条件を明確にしました。
- Obsession Vaultを含め、Obsessing Spark生成時の全画面Moonrise演出を削除しました。
- 静止画を使用するIncubus Butcherの旧アニメーションパッチを無効にしました。
- 対象選択、複製、Frantic処理で破壊済みユニットにアクセスしないよう保護を追加しました。
- **Trainworks Reloaded 0.7.33**と**Conductor 0.5.15**以降が必要です。

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

<!-- 2026-10-27-2301||codex-succclan-targetless||CHANGELOG.md||Ampliar 0.1.12 EN/ZH/ES con Profane enemigo, Frantic sin pérdida si no ataca, autoataque y singular Crown -->

<!-- 2026-10-27-2323||codex-succclan-targetless||CHANGELOG.md||Adoptar formato FreeCompany: índice de idiomas, secciones por idioma, última 0.1.12 en diez idiomas e historial EN/ES; preservar histórico chino -->

<!-- 2026-10-27-2325||codex-succclan-targetless||CHANGELOG.md||Añadir 0.1.13 en diez idiomas con único cambio real frente a publicada: formato/navegación/traducción del changelog; conservar 0.1.12 e historial -->
