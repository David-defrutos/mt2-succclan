using BepInEx;
using BepInEx.Logging;
using System.IO;
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
            new HarmonyLib.Harmony(MyPluginInfo.PLUGIN_GUID).PatchAll();

            var builder = Railhead.GetBuilder();
            builder.Configure(
                MyPluginInfo.PLUGIN_GUID,
                c =>
                {
                    // OJO: Trainworks Reloaded NO escanea carpetas.
                    // Un JSON que no este en esta lista no existe para el juego,
                    // y no da ningun error. Ver docs\64-anadir-json-nuevos-y-recompilar.md
                    string[] jsonPaths =
                    {
                        "json/plugin.json",
                        "json/audiovisual.json",

                        // Clase
                        "json/class.json",

                        // Esencias
                        "json/essences.json",

                        // Estados propios
                        "json/status_effects/psionic.json",
                        "json/status_effects/frantic.json",

                        // Campeones
                        "json/champions/champion_KnightMare.json",
                        "json/champions/champion_ShadowLady.json",

                        // Hechizos y blights
                        "json/spells/card_BloodCarnival.json",
                        "json/spells/card_CubusSpike.json",
                        "json/spells/card_DangerousGame.json",
                        "json/spells/card_DarkFury.json",
                        "json/spells/card_DarkPact.json",
                        "json/spells/card_DepressionWhisper.json",
                        "json/spells/card_DreadShot.json",
                        "json/spells/card_Flogging.json",
                        "json/spells/card_ForTheQueen.json",
                        "json/spells/card_IllusionTwins.json",
                        "json/spells/card_Inception.json",
                        "json/spells/card_InsanityReach.json",
                        "json/spells/card_MindBurning.json",
                        "json/spells/card_MindDomination.json",
                        "json/spells/card_ObsessingShard.json",
                        "json/spells/card_PainAndPleasure.json",
                        "json/spells/card_ParadoxTome.json",
                        "json/spells/card_PiercingShriek.json",
                        "json/spells/card_PlagueBoost.json",
                        "json/spells/card_PowerSiphon.json",
                        "json/spells/card_ProfaneAscending.json",
                        "json/spells/card_ShadowEmbrace.json",
                        "json/spells/card_VitalityExtraction.json",

                        // Salas y equipos
                        "json/rooms/room_SpectralRefuge.json",
                        "json/rooms/room_ObsessionVault.json",
                        "json/equipment/equip_WhisperingBlade.json",
                        "json/equipment/equip_MourningVeil.json",

                        // Reliquias
                        "json/relics/relic_AbyssCrown.json",
                        "json/relics/relic_DemonBlood.json",
                        "json/relics/relic_DesireCrystal.json",
                        "json/relics/relic_FlareRibbon.json",
                        "json/relics/relic_FleshRing.json",
                        "json/relics/relic_MutantElixirs.json",
                        "json/relics/relic_NetherBlossom.json",
                        "json/relics/relic_ObsessingAromatherapy.json",
                        "json/relics/relic_PoisonSerum.json",
                        "json/relics/relic_ProfaneCrossbow.json",
                        "json/relics/relic_ShadowCloak.json",

                        // Unidades
                        "json/units/unit_AbyssPrincess.json",
                        "json/units/unit_ArroganceGhost.json",
                        "json/units/unit_ChaosCreation.json",
                        "json/units/unit_DemonPioneer.json",
                        "json/units/unit_EndlessShadow.json",
                        "json/units/unit_EnvyGhost.json",
                        "json/units/unit_GluttonyGhost.json",
                        "json/units/unit_GreedGhost.json",
                        "json/units/unit_IncubusButcher.json",
                        "json/units/unit_LustGhost.json",
                        "json/units/unit_Oolioddroo.json",
                        "json/units/unit_ShadowWarrior.json",
                        "json/units/unit_SlothGhost.json",
                        "json/units/unit_SuccbusTorturer.json",
                        "json/units/unit_Vrolikai.json",
                        "json/units/unit_WrathGhost.json"
                    };
                    // Trainworks silently ignores missing merged JSON files.
                    // Validate against the DLL directory, the same base path it uses.
                    var contentDirectory = Path.GetDirectoryName(typeof(Plugin).Assembly.Location)!;
                    foreach (var jsonPath in jsonPaths)
                    {
                        var fullPath = Path.Combine(contentDirectory, jsonPath);
                        if (!File.Exists(fullPath))
                        {
                            Logger.LogError($"SuccClan content file missing: {fullPath}. Reinstall the complete mod package; keep json/ and textures/ beside the DLL.");
                            throw new FileNotFoundException("SuccClan content is incomplete or incorrectly installed.", fullPath);
                        }
                    }
                    c.AddMergedJsonFile(source =>
                    {
                        source.Paths = new System.Collections.Generic.List<string>(jsonPaths);
                        source.Optional = false;
                    });
                    Logger.LogInfo($"SuccClan configured {jsonPaths.Length} JSON files from {contentDirectory}.");
                }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
