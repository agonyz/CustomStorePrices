using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CustomStorePrices.Patches;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomStorePrices
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "Agonyz.CustomStorePrices";
        private const string modName = "Custom Store Prices";
        private const string modVersion = "1.0.0";

        private readonly Dictionary<string, ConfigEntry<int>> configEntries = new Dictionary<string, ConfigEntry<int>>();
        private readonly ConfigEntry<int> WalkieTalkie;
        private readonly ConfigEntry<int> Flashlight;
        private readonly ConfigEntry<int> Shovel;
        private readonly ConfigEntry<int> LockPicker;
        private readonly ConfigEntry<int> ProFlashlight;
        private readonly ConfigEntry<int> StunGrenade;
        private readonly ConfigEntry<int> Boombox;
        private readonly ConfigEntry<int> TZPInhalant;
        private readonly ConfigEntry<int> ZapGun;
        private readonly ConfigEntry<int> Jetpack;
        private readonly ConfigEntry<int> ExtensionLadder;
        private readonly ConfigEntry<int> RadarBooster;
        private readonly ConfigEntry<int> SprayPaint;

        private readonly Harmony harmony = new Harmony(modGUID);
        internal static Plugin Instance;
        internal static ManualLogSource Log;

        void Awake()
        {
            if (Instance == null) {
                Instance = this;
            }

            Log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            Log.LogInfo($"{modGUID} was loaded!");

            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(TerminalSetItemSalesPatch));

            InitializeConfigEntries();
        }

        void InitializeConfigEntries()
        {
            var configEntryFields = typeof(Plugin).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(f => f.FieldType == typeof(ConfigEntry<int>));

            foreach (var configEntryField in configEntryFields) {
                var configEntry = Config.Bind("CustomStorePrices.Settings", configEntryField.Name, 1, "Price for " + configEntryField.Name);
                configEntries.Add(configEntryField.Name, configEntry);
            }
        }

        public Dictionary<string, ConfigEntry<int>> GetConfigEntries()
        {
            return configEntries;
        }
    }
}
