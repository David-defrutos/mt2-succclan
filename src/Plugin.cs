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
                        "json/spells/card_ProfaneAscendingPlus.json",
                        "json/spells/card_ShadowEmbrace.json",
                        "json/spells/card_VitalityExtraction.json",

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
                    );
                }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
