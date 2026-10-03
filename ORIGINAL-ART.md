# Restore the original Monster Train 1 artwork / Recuperar el arte original de MT1

The original artwork belongs to [CodePointer's SuccClan](https://github.com/CodePointer/SuccClan). This package uses redrawn card and character art. To restore the original art in your own installation:

1. Download the original repository from the green **Code → Download ZIP** button on GitHub and extract it.
2. Find your installed MT2 SuccClan folder in your Thunderstore profile, then back up its `textures` directory.
3. For each image you want to change, find the corresponding original PNG in the table below. Resize it to match the **pixel dimensions and transparent canvas** of the MT2 target PNG, keeping the subject centered and its feet at the same height as in the MT2 image. Save as PNG and replace the MT2 file with the exact target filename. Card art and character art are separate images.
4. Restart Monster Train 2. Thunderstore updates may overwrite your replacements; keep a backup.

| MT2 target in `textures/` | MT1 source in `Assets/` |
| --- | --- |
| `KnightMareCardArt.png` | `CardAssets/UnitCardArt/Champion_KnightMare.png` |
| `KnightMareCharacterArt.png` | `CardAssets/UnitPortrait/Champion_KnightMare.png` |
| `ShadowLadyCardArt.png` | `CardAssets/UnitCardArt/Champion_ShadowLady.png` |
| `ShadowLadyCharacterArt.png` | `CardAssets/UnitPortrait/Champion_ShadowLady.png` |
| `{UnitName}CardArt.png` | `CardAssets/UnitCardArt/Unit_{UnitName}.png` |
| `{UnitName}CharacterArt.png` | `CardAssets/UnitPortrait/Unit_{UnitName}.png` |
| `{GhostName}GhostCardArt.png` | `CardAssets/UnitCardArt/Subunit_{GhostName}Ghost.png` |
| `{GhostName}GhostCharacterArt.png` | `CardAssets/UnitPortrait/Subunit_{GhostName}Ghost.png` |
| `{SpellName}Art.png` | `CardAssets/SpellCardArt/Spell_{SpellName}.png` |
| `{RelicName}Art.png` | `Relic/Relic_{RelicName}.png` |

Exceptions: `ObsessingShardArt.png` comes from `CardAssets/SpellCardArt/Blight_ObsessingShard.png`; `MindDominationArt.png` comes from the original file `Spell_MindDomaination.png` (the original spelling); the target `SuccbusTorturer` keeps that exact spelling. Newly added art without a corresponding MT1 image cannot be restored from the original repository. Keep the original PNG's transparency when exporting character art.

---

El arte original está en el [repositorio SuccClan de CodePointer](https://github.com/CodePointer/SuccClan). Este paquete utiliza dibujos rehechos para cartas y personajes. Para recuperar las imágenes de MT1 en tu instalación:

1. Descarga el ZIP del repositorio original desde **Code → Download ZIP** en GitHub y descomprímelo.
2. Busca la carpeta de SuccClan instalada en tu perfil de Thunderstore y haz una copia de seguridad de `textures`.
3. Busca la imagen original con la tabla anterior. Ajústala a las **dimensiones y al lienzo transparente** del PNG de MT2, centrando el personaje y manteniendo los pies a la misma altura. Guárdala como PNG y reemplaza el fichero de MT2 con su nombre exacto. El arte de carta y el del personaje son ficheros distintos.
4. Reinicia Monster Train 2. Una actualización del paquete puede sobrescribir los cambios, así que conserva la copia de seguridad.

Hay tres excepciones en la nota bajo la tabla: `ObsessingShardArt`, `MindDominationArt` (con una errata en el nombre original) y `SuccbusTorturer` (se conserva el nombre exacto). Las imágenes nuevas que no existían en MT1 no pueden recuperarse del repositorio original.
