<!-- Documento generado el 2026-09-13-2045 -->

# De donde sale cada cosa

## Origen

Este clan es un **traspaso a Monster Train 2** del clan SuccClan de Monster Train 1, de
**CodePointer**:

- https://github.com/CodePointer/SuccClan (version 0.1.4, commit final `Finish 0.1.4`,
  26-feb-2022)
- Espejo identico del mismo autor: https://github.com/CodePointer/CubusClan
- El item de Steam Workshop (`2762086278`) esta retirado por Steam y ya no se puede bajar.

Licencia del original: **MIT**. El `LICENSE` de esta carpeta conserva su linea de copyright
y anade la mia debajo, como pide la licencia.

## Que se reutiliza tal cual

- **El arte**: los 88 PNG de `Assets\` del repo original, reescalados a los tamanos que pide
  MT2 (`docs\50-arte-escalas.md`). Sin retocar nada mas.
- **Los textos**: nombres y descripciones de carta salen de las hojas `CSV` y `CSV (2)` del
  `DesignDoc.xlsx` del autor, 227 entradas en seis idiomas. Volcadas en
  `D:\Juegos\MT2_mod\_upstream\succclan\succclan-textos-2026-09-13-1955.csv`.
  El `SuccClan.csv` que el plugin de MT1 importaba no esta en el repositorio.

## Que NO se reutiliza

Nada de C#. Los ~9.300 lineas del original -15 efectos de carta propios, 3 traits, 10
efectos de reliquia, 2 estados y un trigger con dos parches Harmony- se sustituyen por el
vocabulario que MT2 y Conductor ya traen de serie. El mapeo completo, efecto a efecto, en
`docs\79-port-succclan.md`.

La unica excepcion es `src\code\StatusEffectMarkerState.cs`, una clase **vacia**: Trainworks
Reloaded exige un `class_name` en todo `status_effect` propio, asi que un estado de solo
tooltip no se puede declarar unicamente en JSON.

## Desviaciones deliberadas respecto al original

| que | en MT1 | aqui | por que |
|---|---|---|---|
| trigger **OnFanatic** | dos parches Harmony sobre `CardManager` | `@Accursed` de Conductor | Conductor ya detecta que se juegue o descarte un Blight o un Scourge |
| estado **Frantic** | golpea al aliado del frente con su propio ataque | marcador + `dazed` y `witherbloom` desde las cartas | no hay equivalente en MT2 y no se quiere C# |
| **Vengeful Shard** | blight del juego base de MT1 | el propio `Obsessing Spark` del clan | el id de MT1 no existe en MT2 |
