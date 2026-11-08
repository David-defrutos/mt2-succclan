<!-- Documento generado el 2026-11-07-1323 -->

# Desarrollo local de SuccClan

Fuente única editable y origen de los commits/push: `D:\Juegos\MT2_mod\repos\MT2\variaciones-mias\mt2-succclan`. Default es la copia de ejecución usada por David: no editar fuentes allí, crear perfiles ni activar alternativas. Descargar de Thunderstore puede sustituirla; para volver al desarrollo se reinstala desde este repositorio sobre esa misma copia.

## Acciones independientes

Validar (compilar y ejecutar comprobaciones; **no instala**):

```powershell
python 'D:\Juegos\MT2_mod\repos\MT2\variaciones-mias\mt2-succclan\tools\develop.py' validate
```

Instalar para probar, con Monster Train2 cerrado:

```powershell
python 'D:\Juegos\MT2_mod\repos\MT2\variaciones-mias\mt2-succclan\tools\develop.py' install
```

Valida, respalda archivos propios, copia DLL/JSON/texturas/metadata a `C:/Users/david/AppData/Roaming/Thunderstore Mod Manager/DataFolder/MonsterTrain2/profiles/Default/BepInEx/plugins/David-SuccClan_Custom`, verifica bytes y retira solo obsoletos propios del inventario. Conserva archivos ajenos. No instala fuentes, Git, tests ni dependencias sobre otros mods. Juego cerrado comprobado antes y después de validar. Inventario inicial tools/default-runtime-files.json; actualizaciones en .build/default-installed-files.json. El backup queda en .build/<ejecucion>/before-install.zip. Si hay cambios externos o falla una copia, parar; no forzar.

Preparar publicación (valida y genera ZIP; **no instala ni publica**):

```powershell
python 'D:\Juegos\MT2_mod\repos\MT2\variaciones-mias\mt2-succclan\tools\develop.py' package
```

Usa staging y DLL recién compilada, `salidas/thunderstore/<fecha-Madrid>/<ejecucion>/`, sin sobrescribir ZIP existentes. Directorio opcional --output. Los informes result.json identifican commit, cambios locales, hash de entradas, DLL y ZIP. Un árbol sucio no se presenta como idéntico al commit. Ninguna acción hace commit/push/publicación.

## Build y comprobaciones

Python con Pillow y dotnet. Proyecto autocontenido tools/local-build/SuccClan.Local.csproj con DLL instaladas de BepInEx, TW0.7.35, Conductor0.5.17 y Managed del juego. Restore sin feeds con tools/local-build/NuGet.Config. Fuentes y tests enlazan src/ del mismo repositorio, nunca Default. Al cambiar versión, alinear manifest, proyecto principal, proyecto local y tools/local-build/MyPluginInfo.cs. Los62 JSON cargados están registrados explícitamente en src/Plugin.cs.

Validate ejecuta 778 contratos de contenido,292 pruebas simuladas existentes,95 de Shadow,79 de API nativa,6 pruebas de instalación aislada y validación audiovisual. Las suites y fuentes están dentro de tests/ y tools/. Las salidas .build/, bin/, obj/ y caches quedan ignoradas. Las pruebas aisladas no sustituyen Unity.

## Consolidación realizada el 07-11-2026

Comparadas y respaldadas ambas carpetas completas, incluidos Git e ignorados: backups/zips/SuccClan-before-consolidation-installed-2026-11-07-1203.zip y SuccClan-before-consolidation-target-2026-11-07-1203.zip. Destino limpio en0112f23 avanzado con --ff-only desde el repositorio LOCAL instalado a5ed81bd, sin descargar fuentes de GitHub. Los293 archivos versionados se preservaron byte a byte, además del arte y recursos locales.

Desarrollo antiguo fuera de Default en `D:/Juegos/MT2_mod/backups/mods/SuccClan-development-retired-2026-11-07-1319`. Git oculto archivado y comprobado antes de retirarlo; primer Move-Item detenido por Hidden, sin pérdida. Worktree histórico de laboratorio con puntero reparado hacia metadata archivada; no es desarrollo activo.

Runtime de Default sin reinstalar: mantiene DLL SHA02f4db2e y coincide con las251 entradas del ZIP candidato del07-11. Install se probó solo sobre temporales aislados; package se probó hacia tmp. Rebuild externo SHAff83d998 distinto por rutas/PDB/MVID; firmas e IL ejecutable de249 métodos coinciden con candidata (578 verificaciones en esa comparación ampliada). No sustituir el ZIP de subida por el ZIP temporal de prueba.

Los actualizadores ya protegen este repositorio: avance rápido de rama activa, conservar cambios/commits, sin reset/clean. Procedimiento: D:/Juegos/MT2_mod/docs/guias/actualizar-repositorios-trabajo-2026-11-07-1152.md. README público del mod intacto. Cambios del flujo aún sin nuevo commit/push; no se ha publicado.

<!-- 2026-11-07-1323||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\COMO-COMPILAR.md||Documentar fuente unica externa y separar validar, instalar y empaquetar -->

La instalación selecciona la única copia existente con mt2_succclan.Plugin.dll ejecutable entre frutos-SuccClan y David-SuccClan_Custom. Nunca activa una DLL .old ni modifica mods.yml. Si no hay un único destino inequívoco, se detiene sin cambiar configuración. En esta consolidación se conserva la copia usada; descargar normalmente de Thunderstore y cambiar el estado desde el gestor sigue siendo decisión del usuario.

<!-- 2026-11-07-1325||codex-succclan-targetless||repos\MT2\variaciones-mias\mt2-succclan\COMO-COMPILAR.md||Aclarar seleccion de copia usada sin activar alternativas y actualizar pruebas de aislamiento -->
