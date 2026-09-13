<!-- Documento generado el 2026-09-13-2045 -->

# Compilar el DLL de este mod

> El plugin lleva en `src\Plugin.cs` la **lista de ficheros JSON que carga**. Un JSON nuevo
> no existe para el juego hasta que aparece en esa lista y se recompila.
> Editar un JSON que ya esta en la lista no requiere compilar nada.

A diferencia de Silksong, Sandscourged y Yokai, **este clan no es un fork**: no hay upstream
en MT2 del que salga el `src\`. Esta escrito de cero a partir del mod de MT1 de CodePointer.
Detalle en `NOTICE.md` y en `D:\Juegos\MT2_mod\docs\79-port-succclan.md`.

Rutas JSON declaradas ahora mismo: **7**.

## 1. Anadir un JSON nuevo

Abre `src\Plugin.cs`, busca el bloque `c.AddMergedJsonFile(` y mete la ruta en el grupo que
toque. Las rutas son relativas a esta carpeta:

```csharp
// Unidades
"json/units/unit_GreedGhost.json",
"json/units/unit_LustGhost.json",
```

El fichero va en **CRLF**. Si lo reescribes en LF el diff sale entero y no hay quien lo
revise.

## 2. Compilar

### 2.1. En GitHub Actions (el camino corto)

Empuja a `main` cualquier cambio dentro de `src\` y el workflow `Build DLL` arranca solo.
Tambien se lanza a mano desde **Actions -> Build DLL -> Run workflow**.

Actions resuelve las credenciales de `nuget.pkg.github.com` con el `GITHUB_TOKEN` del propio
runner, y en repos publicos es gratis e ilimitado.

El resumen del run dice **cuantas rutas JSON declara tu Plugin.cs**. Si anades un fichero y
se te olvida la linea, ese numero te lo canta sin abrir nada.

### 2.2. En local

Necesitas el **SDK de .NET 9**. El `src\nuget.config` apunta a dos fuentes, y una de ellas
(`nuget.pkg.github.com`) pide credenciales de GitHub aunque el paquete sea publico. Con un
token personal con permiso `read:packages`:

```powershell
$mod = "C:\Users\david\AppData\Roaming\Thunderstore Mod Manager\DataFolder\MonsterTrain2\profiles\Default\BepInEx\plugins\David-SuccClan_Custom"
dotnet nuget update source monster-train-packages -u TU_USUARIO -p TU_TOKEN --store-password-in-clear-text --configfile "$mod\src\nuget.config"
dotnet restore "$mod\src"
dotnet build "$mod\src" -c Release --output D:\Juegos\MT2_mod\_dll-build\out
```

## 3. Instalar el DLL

**La salida del build no puede quedarse dentro de `plugins\`.** BepInEx escanea esa carpeta
en profundidad buscando DLLs, y una copia en `src\bin\` haria que cargase el plugin dos
veces. Por eso el `--output` apunta fuera y luego se copia solo el DLL.

Con el GitHub CLI, que es lo fiable (el ZIP de Actions baja de 0 bytes si el navegador no
tiene sesion):

```powershell
$mod = "C:\Users\david\AppData\Roaming\Thunderstore Mod Manager\DataFolder\MonsterTrain2\profiles\Default\BepInEx\plugins\David-SuccClan_Custom"
cd $mod
gh run download --name mt2_succclan.Plugin --dir "D:\Juegos\MT2_mod\ddls\dll-succclan"
Copy-Item "D:\Juegos\MT2_mod\ddls\dll-succclan\mt2_succclan.Plugin.dll" $mod -Force
```

La primera vez no hay copia de seguridad que hacer, porque no hay DLL previo. A partir de la
segunda, **siempre antes**:

```powershell
Copy-Item "$mod\mt2_succclan.Plugin.dll" "D:\Juegos\MT2_mod\backups\dll-succclan-anterior.dll"
```

## 4. Comprobar

```powershell
$log = "$env:USERPROFILE\AppData\LocalLow\Shiny Shoe\MonsterTrain2\Player.log"
Select-String -Path $log -Pattern "CATASTROPHIC|Exception|Failed to load" | Select-Object -First 10
Select-String -Path $log -Pattern "Succubus|Flogging|GreedGhost|ObsessingShard|psionic|frantic" |
    Select-Object -ExpandProperty Line
& D:\Juegos\MT2_mod\scripts\validate-mt2-mods.ps1
```

Para cada carta nueva tienen que salir, por este orden: `CardUpgradeRegister`,
`SpriteRegister`, `GameObjectRegister`, `CardDataRegister` y al final `CardDataFinalizer`.
Si falta el `Finalizer`, el fichero se carga pero algo de dentro no cuadra.

Lo que hay que mirar especificamente en esta primera vuelta:

1. Que la clase **Succubus** aparece en la pantalla de seleccion de clan, aunque todavia no
   tenga campeones.
2. Que **Obsessing Spark** danya la pira al quedarse en la mano al final del turno
   (`on_unplayed_negative`).
3. Que **Flogging** mete de verdad la carta en el mazo: `param_int: 7` es `DeckPileRandom`
   y `param_int_2` el numero de cartas. Si falta el segundo, el efecto no anade nada y no
   avisa.
4. Que el trigger **`@Accursed`** de Conductor dispara en el Greed Ghost al descartar el
   Obsessing Spark. Es la pieza que sustituye al OnFanatic del original.

## 5. Marcha atras

Renombrar el DLL a `.dll.off`. Renombrar la carpeta **no vale**: BepInEx la rastrea
recursivo. Detalle en `D:\Juegos\MT2_mod\docs\61-mods-versiones-y-perfiles.md`.
