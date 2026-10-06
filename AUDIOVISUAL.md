# SuccClan: efectos visuales y sonidos

Implementación local: **04-10-2026**, sobre la versión **0.1.4** preparada.
Solo presentación: estadísticas, objetivos, costes, triggers, traducciones y
reglas de las cartas se conservan.

## Recursos y asignaciones

`json/audiovisual.json` define 18 recursos, con rutas y GUID del juego instalado.
El plugin lo registra junto a los otros 61 JSON. No se incluyen ni redistribuyen
archivos de audio o prefabs del juego, ni se ha generado arte nuevo.

| Grupo | Recursos existentes utilizados |
| --- | --- |
| Ghosts | Ataque/impacto de Ethereal Seelie, proyectil de Purified Soul y humo de muerte |
| Knightmare, Oolioddroo, Chaos Creation, Abyss Princess | Ataque de Ekka e impacto mágico; Oolioddroo también tiene proyectil |
| Otras unidades y Shadow Lady | Ataque de Death's Dancer e impacto de sombras |
| Todas las unidades | Humo al morir y aparecer, también en clones/reapariciones |
| Daño directo, Dread Shot, Flogging, Envy, Sloth, Profane Crossbow | Shadow Impact |
| Power Siphon, Piercing Shriek y Oolioddroo | Ancient Magic Impact |
| Buffs/debuffs directos | Attack Buff / Health Buff o impacto de sombras para reducciones |
| Curación | Heal Impact Normal; Heal Impact Small en Mourning Veil |
| Cubus Spike | Rail spike de Luna Coven en la aplicación de Frantic |
| Psionic | Pulso de Fae's Blessing al ganar cargas y en sus efectos de gasto |
| Frantic | Aplicación e impacto de sombras en la víctima real |
| Obsession y generación de Sparks | Pulso de Moonrise en la sala |
| Pain and Pleasure, Plague Boost | Pulso de Alchemy en la sala, sin sugerir un objetivo de unidad |
| Depression Whisper | Echo Snare al retirar buffs, sin repetirlo por cada debuff |
| For the Queen | Impacto de sombras en el sacrificio |
| Spectral Refuge | Sanctuary al instalar y activar la sala; buff de vida y Regen nativo en los Ghosts |
| Obsession Vault | Moonrise al instalar y activar; un solo pulso por turno |
| Whispering Blade | Attack Buff al equipar y Psionic al activarse Strike |
| Mourning Veil | Health Buff al equipar y curación pequeña en Accursed |

Mind Burning y Vitality Extraction conservan los recursos de sus estados
nativos; reciben también los de Frantic/Psionic cuando corresponde. Mutated es
una marca de seguimiento interna y conserva su presentación de Multistrike.
Los artefactos pasivos conservan sus respuestas nativas; se añadió el impacto
del disparo de Profane Crossbow.

## Sonido y animación

### Cartas de los dos campeones

`ChampionCardAnimation.cs` añade una animación exclusiva a los prefabs
`KnightMareCardArt` y `ShadowLadyCardArt`, después de que Trainworks cree su arte.
La comprobación de namespace, tipo `card_art` e ID evita aplicarla a personajes
de combate, retratos de selección o cartas de otros clanes.

El retrato tiene un movimiento suave y zoom entre 1,020 y 1,036, con un ciclo
de 6,6 segundos en Knightmare y 7,4 en Shadow Lady. La deriva máxima es 0,3%
horizontal y 0,4% vertical. Una máscara limita la animación al rectángulo del
arte de la carta; marco, título y texto permanecen en su posición.

Un halo palpita cerca de la magia de la mano y 12 pequeñas motas ascienden
por los laterales: azul/cian para Knightmare, rosa/violeta para Shadow Lady.
Se dibujan como geometría de UI; no requieren PNG nuevos ni modifican los
originales. Es una animación ambiental del retrato existente, sin nuevos
fotogramas de poses ni movimiento articulado del personaje.

Se conservan el color/alpha que controla CardUI y la animación de interacción
de la carta. La capa de luz no recibe clics, comparte la máscara y respeta el
tinte del retrato. No usa aleatoriedad del juego ni materiales compartidos.
Las cartas ocultas o sin Canvas no reconstruyen la geometría cada frame.

Build local: 0 errores y 0 avisos. Los JSON y el arte original no cambian.
Pendiente ver la animación en mano, selección y Compendium dentro del juego.

Se conservan los cues nativos de spawn, daño recibido, muerte, curación,
buffs, estados y operaciones de mano. Psionic usa `Combat_Buff` al aplicarse;
Frantic usa `Combat_Debuff` al aplicarse.

`SuccClanPresentation.cs` añade `Combat_Attack` a daño de hechizos sin atacante
propio y `Combat_Ascend` a movimientos por hechizo con objetivos. No añade un
segundo sonido cuando el daño ya tiene un atacante nativo, ni sonido de ataque
para la Spark no jugada que daña la Pyre. El daño de sala recibe una señal por
aplicación del efecto, no otra por cada objetivo.

Frantic deja de emitir `DamageAppliedPlaySound(DirectAttack)`, una señal que el
gestor de sonido ignoraba. Cada golpe real solicita `Combat_Attack`; si la
unidad está libre para animarse, también tiene windup y movimiento de ataque.
Se suprimen sus partículas/proyectiles hacia el enemigo: el impacto se presenta
en el aliado o en la propia unidad. Multistrike, selección de objetivo, daño,
consumo de una carga y exclusión de bosses mantienen las reglas anteriores.

La espera de animación tiene un límite y se interrumpe al morir. Si la unidad
ya está animándose, se conserva esa animación y se reproduce el sonido/impacto.

## Triggers, rooms y duplicaciones

La presentación usa `CardEffectState.PlayEffectVfx`, el método nativo de la UI.
Se conserva el callback original cuando existe. Si falta en un contexto de
SuccClan, se añade la presentación; las rooms proporcionan su callback explícito.
El identificador nativo `appliedVfxId` evita duplicar el mismo impacto cuando la
clase de daño vuelve a solicitarlo.

La generación de Spark de Obsession Vault no lleva otro `applied_vfx`, porque
su modificador ya emite Moonrise. Se conservan los VFX de Regen, Armor, Rage,
Multistrike y demás estados nativos. No se añadieron auras permanentes.

Los hooks están limitados a cartas/personajes de SuccClan y sus rooms. Preview
y Compendium no reproducen la presentación añadida. Los fallos de presentación
auxiliar se registran una vez por tipo y permiten continuar el efecto de juego.

## Verificación realizada y límites

- Compilación con las DLL instaladas: **0 errores, 0 avisos**.
- Proveedor real de configuración de Trainworks: 62 JSON, 46 cartas,
  18 personajes, 110 sprites y 1 clan.
- 18 rutas/GUID contrastados con el catálogo audiovisual local; presencia
  comprobada también en el catálogo actual del juego.
- 152 referencias audiovisuales resueltas; todos los recursos definidos usados.
- Comparación con la copia previa: **61 JSON conservan exactamente sus datos
  de juego y traducciones**, al excluir los campos de presentación añadidos.
- 25 comprobaciones de comportamiento sobre el código C# real con sustitutos
  del motor: Frantic, preview, muerte propia, bosses, scoping, callback nativo,
  sonido único y spawn.
- Validador habitual: nada que rompa la carga; DLL y disco coinciden en 62 JSON.

**Pendiente probar en partida:** aspecto y sonido real, tamaño de partículas,
anclas, duración, comportamiento a velocidad rápida y reapariciones. Las
pruebas con sustitutos no ejecutan Unity ni reproducen audio.

## Validación reproducible

```powershell
python tools/validate_audiovisual.py --catalog "C:\Juegos\Steam\steamapps\common\Monster Train 2\MonsterTrain2_Data\StreamingAssets\aa\catalog.json"
```

Para comprobar que esta tanda conserva los valores previos:

```powershell
python tools/validate_audiovisual.py --baseline "D:\Juegos\MT2_mod\backups\2026-10\succclan-fx-20261004-144309"
```

La copia incluye el DLL, los JSON y los archivos C# anteriores. Los tests locales
están en `D:\Juegos\MT2_mod\tmp\succclan-presentation-tests`; los resultados
y el inventario aplicado se conservan en la documentación del workspace.

El DLL está instalado en el mod principal. Esta tanda no hace commit, push ni
publicación en Thunderstore.

## Cartas raras animadas: 04-10-2026

Animación ambiental solo de `card_art`, añadida a las diez cartas de rareza
`rare` del clan; conserva las animaciones de los dos campeones. Zoom/deriva
suaves, luces sobre la magia del dibujo y doce motas laterales. Marco, texto,
arte de combate, PNG, valores de juego y traducciones sin cambios.

| Carta | Luz |
|---|---|
| Cubus Spike | Violeta, punta e impacto del clavo |
| Depression Whisper | Lavanda, susurro espectral |
| Illusion Twins | Cian, silueta ilusoria |
| Insanity Reach | Roja, remolinos mágicos |
| Paradox Tome | Cian, magia del libro |
| Plague Boost | Verde, humo y caldero |
| Abyss Princess | Rosa, magia de la mano |
| Arrogance Ghost | Rosa, aura y cola espectral |
| Endless Shadow | Violeta, estela y sombra trasera |
| Shadow Warrior | Violeta, hojas mágicas |

Compilación local: 0 errores y 0 avisos. IDs contrastados con todas las cartas
raras en JSON y hashes de JSON originales sin cambios. Pendiente comprobar
el resultado visual en el juego. Sin commit, push ni publicación.

Estado del DLL: instalado; reiniciar el juego para cargarlo.
