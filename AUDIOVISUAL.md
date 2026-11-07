<!-- Documento generado el 2026-11-07-1141 -->

# SuccClan: presentación audiovisual vigente

Estado de la candidata 0.1.14. El historial de implementaciones retiradas está en el documento `archive/audiovisual-history-2026-11-07-1141.md` del repositorio fuente.

## Efectos y sonidos activos

- `json/audiovisual.json` conserva 18 definiciones de recursos nativos; no se redistribuyen prefabs ni audio del juego.
- Ghosts: ataque/impacto espectral, proyectil Purified Soul y humo de aparición/muerte. El resto de unidades utiliza sus ataques, impactos y muerte asignados.
- Psionic: pulso Fae's Blessing y `Combat_Buff`. Frantic: impacto de sombras y `Combat_Debuff` al aplicarse. Sus ataques, animación, daño y predicción los ejecuta RunUnitTurn nativo.
- Hechizos de daño: Shadow Impact o Ancient Magic Impact según carta. Daño sin atacante propio puede usar `Combat_Attack`; movimiento con objetivos usa `Combat_Ascend`.
- Cura, Armor, Regen, Rage, Sap, Multistrike y operaciones de mano conservan sus respuestas nativas.
- Obsession y generación de Sparks: no usan el pulso Moonrise de pantalla completa. Su definición retirada permanece sin referencias activas.
- Obsession Vault: no reproduce Moonrise al instalarse ni al generar su Spark.
- Spectral Refuge: pulso Sanctuary al instalar/activar, Health Buff y Regen en Ghosts.
- Whispering Blade: Attack Buff al equiparse; **Accursed: Sap 2 al enemigo trasero**. No concede Psionic ni tiene un trigger Strike.
- Mourning Veil: Health Buff al equiparse y curación pequeña con Accursed.
- Desire Crystal: notificación localizada de robo de 1 carta y ganancia de 1 ember al primer Ghost del turno. Se conserva su filtro y límite nativo.

## Arte y animación

- Las cartas de los dos campeones y las diez raras usan una animación ambiental de retrato y brillo. Se conectan mediante `Roundhouse.WhenReady` y la API pública `GetCardArt`, sin parchear el configurador interno de Trainworks.
- El movimiento está limitado al rectángulo del arte; marco, nombre y texto pertenecen a CardUI. Se conserva el tinte/alpha del juego y no se usa RNG de combate.
- **Incubus Butcher utiliza una imagen estática.** Su antigua animación de combate y sus cuatro parches están archivados/comentados, sin registrarse. Las animaciones de cartas son independientes.
- **Curseguard Stone y Curseguard Slab tienen iconos propios**, con basalto violeta, sigilo de escudo/llama espectral y transparencia. Slab se distingue por la losa reforzada con metal. Exportados a 256×256 y 96×96 en `textures/RelicIcons/`; no reutilizan Shadow Cloak.

## Integración y límites

`SuccClanPresentation.cs` completa presentación en contextos SuccClan identificados por clase y asset key. Conserva callbacks existentes y usa `appliedVfxId` nativo para evitar impactos duplicados. Preview y Compendium no reproducen los sonidos/efectos auxiliares. Las salas delegan en efectos nativos y proporcionan su callback visual.

La compilación, las comprobaciones de recursos y la inspección de los PNG no sustituyen una prueba en Unity. Comprobar tamaños dentro de tienda, iconos pequeños, tintes, velocidades de juego y notificaciones. La predicción de Endless Shadow sincroniza ahora la penalización de vida en la carta temporal de cada copia; el spawner nativo sigue aplicando los cambios al personaje. Sus pruebas aisladas no sustituyen comprobar Relentless en partida.
<!-- 2026-11-07-1141||codex-succclan-targetless||AUDIOVISUAL.md||Reemplazar estado audiovisual desfasado por implementacion vigente; historial preservado en archive -->

<!-- 2026-11-07-1153||codex-succclan-targetless||AUDIOVISUAL.md||Evitar enlace relativo a archive que no se incluye en el ZIP -->

<!-- 2026-11-07-1155||codex-succclan-targetless||AUDIOVISUAL.md||Actualizar estado de prediccion Shadow tras correccion; mantener limites de prueba Unity -->
