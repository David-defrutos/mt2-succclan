# SuccClan for Monster Train 2

## ⚠ ALPHA

This is a playable **ALPHA**. Expect bugs, rough balance and further changes. Reports about broken cards, unclear text and balance are welcome at [GitHub issues](https://github.com/David-defrutos/mt2-succclan/issues); a screenshot, reproduction steps and `BepInEx/LogOutput.log` help.

## Port and credits

This is a port of [SuccClan for Monster Train 1 by CodePointer](https://github.com/CodePointer/SuccClan), made **with the original author's permission**. CodePointer created the original clan, characters, card concepts and art. Thank you for allowing the port. The original project is MIT licensed; attribution details are in [NOTICE.md](https://github.com/David-defrutos/mt2-succclan/blob/main/NOTICE.md) and [LICENSE](https://github.com/David-defrutos/mt2-succclan/blob/main/LICENSE).

## Main differences between MT1 and MT2

- **Artwork:** the card and character artwork has been redrawn in a **SFW (safe-for-work)** style for this MT2 port. I know some players prefer the MT1 artwork. You can restore CodePointer's original images locally with the [original art replacement guide](https://github.com/David-defrutos/mt2-succclan/blob/main/ORIGINAL-ART.md). The game mechanics do not depend on which art you use.
- **Knightmare, Endless path:** instead of spawning a copy of itself on death, it uses Stasis and gains permanent stats by spending Psionic. The path also has a new active ability, Psionic Collapse.
- **Knightmare, Abyss and Illusion paths:** their Psionic Burst effects were rebuilt for MT2. Abyss uses Conductor's Accursed trigger for Blight/Scourge interactions; Illusion adds Spikes alongside Lifesteal.
- **Frantic:** retains the idea of making a unit attack its own front ally, is stackable and consumes one stack when it triggers. Bosses (including minibosses and companion bosses) are immune. In MT1 stacks were removed at turn end.
- Other cards and effects were adapted where MT1 mechanics or targets did not transfer directly to MT2. This release is a reinterpretation, so please report interactions that feel wrong.

Requires BepInExPack, Trainworks Reloaded and Conductor. Source and issues: [mt2-succclan](https://github.com/David-defrutos/mt2-succclan).

---

# SuccClan para Monster Train 2

## ⚠ ALPHA

Esta versión **ALPHA** se puede jugar, pero puede tener fallos y un equilibrio sin pulir. Puedes comunicar errores, textos confusos y problemas de balance en [GitHub issues](https://github.com/David-defrutos/mt2-succclan/issues). Ayudan una captura, los pasos para reproducirlo y `BepInEx/LogOutput.log`.

## Port y créditos

Es un port del [SuccClan de Monster Train 1 creado por CodePointer](https://github.com/CodePointer/SuccClan), hecho **con permiso de su autor original**. CodePointer creó el clan original, los personajes, las ideas de las cartas y su arte. Gracias por permitir el port. El proyecto original usa la licencia MIT; los detalles están en [NOTICE.md](https://github.com/David-defrutos/mt2-succclan/blob/main/NOTICE.md) y [LICENSE](https://github.com/David-defrutos/mt2-succclan/blob/main/LICENSE).

## Cambios principales respecto a MT1

- **Arte:** he rehecho el arte de las cartas y los personajes en una versión **SFW** para este port a MT2. Sé que a algunas personas les gustaba más el arte de MT1. Se pueden recuperar las imágenes originales de CodePointer en la instalación local siguiendo la [guía para sustituir el arte](https://github.com/David-defrutos/mt2-succclan/blob/main/ORIGINAL-ART.md). Las mecánicas no dependen del arte elegido.
- **Knightmare, senda Endless:** en vez de invocar una copia de sí mismo al morir, usa Stasis y gana estadísticas permanentes al consumir Psionic. También tiene la nueva habilidad Psionic Collapse.
- **Knightmare, sendas Abyss e Illusion:** he reconstruido sus efectos Psionic Burst para MT2. Abyss usa el disparador Accursed de Conductor al jugar o descartar Blight/Scourge; Illusion añade Spikes además de Lifesteal.
- **Frantic:** mantiene la idea de atacar al aliado delantero, puede acumular cargas y consume una al activarse. Los jefes (incluidos minibosses y jefes acompañantes) son inmunes. En MT1 las cargas se quitaban al final del turno.
- He adaptado otras cartas y efectos cuando las mecánicas o los objetivos de MT1 no tenían equivalencia directa en MT2. Si alguna interacción parece errónea, indícamela.

Requiere BepInExPack, Trainworks Reloaded y Conductor. Código e incidencias: [mt2-succclan](https://github.com/David-defrutos/mt2-succclan).

The historical binary uses .dll.snapshot so BepInEx cannot load it as a second plugin.
