using BepInEx;
using BepInEx.Logging;
using Microsoft.Extensions.Configuration;
using TrainworksReloaded.Core;
using TrainworksReloaded.Core.Extensions;

namespace mt2_succclan.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);

        public void Awake()
        {
            Logger = base.Logger;

            var builder = Railhead.GetBuilder();
            builder.Configure(
                MyPluginInfo.PLUGIN_GUID,
                c =>
                {
                    // OJO: Trainworks Reloaded NO escanea carpetas.
                    // Un JSON que no este en esta lista no existe para el juego,
                    // y no da ningun error. Ver docs\64-anadir-json-nuevos-y-recompilar.md
                    c.AddMergedJsonFile(
                        "json/plugin.json",

                        // Clase
                        "json/class.json",

                        // Estados propios
                        "json/status_effects/psionic.json",
                        "json/status_effects/frantic.json",

                        // Campeones
                        "json/champions/champion_KnightMare.json",
                        "json/champions/champion_ShadowLady.json",

                        // Hechizos y blights
                        "json/spells/card_ObsessingShard.json",
                        "json/spells/card_Flogging.json",
                        "json/spells/card_Inception.json",

                        // Unidades
                        "json/units/unit_GreedGhost.json"
                    );
                }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
