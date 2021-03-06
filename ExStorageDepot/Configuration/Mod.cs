﻿using ExStorageDepot.Mono;
using FCSCommon.Utilities;
using SMLHelper.V2.Utility;
using System;
using System.Collections;
using System.IO;
using FCSCommon.Extensions;
using Oculus.Newtonsoft.Json;
using SMLHelper.V2.Crafting;
using UnityEngine;

namespace ExStorageDepot.Configuration
{
    internal class Mod
    {
        private static ModSaver _saveObject;
        private static ExStorageDepotSaveData _exStorageDepotSaveData;
        private static TechType _dSSServerTechType;

        internal static event Action<ExStorageDepotSaveData> OnExStorageDepotLoaded;
        
        internal const string SaveDataFilename = "ExStorageDepotSaveData.json";
        internal const string ModName = "ExStorageDepot";
        internal const string ClassID = "ExStorageDepot";
        internal const string ExStorageTabID = "EX";

        internal const string ModFriendly = "Ex-Storage Depot";
        internal const string BundleName = "exstoragedepotunitmodbundle";
        internal const string ModDesc = "Alterra Storage Solutions Ex-Storage Depot allows you to store a large amount of items outside your base.";
        internal static string MODFOLDERLOCATION => GetModPath();
        internal static string GameObjectName => "Ex-StorageDepotUnit";
        internal static string ModFolderName => $"FCS_{ModName}";

#if SUBNAUTICA
        public static TechData ExStorageIngredients => new TechData
        {
            craftAmount = 1,
            Ingredients =
            {
                new Ingredient(TechType.WiringKit, 1),
                new Ingredient(TechType.TitaniumIngot, 2),
                new Ingredient(TechType.Glass, 1),
                new Ingredient(TechType.PowerCell, 1)
            }
        };
#elif BELOWZERO
        public static RecipeData ExStorageIngredients => new RecipeData
        {
            craftAmount = 1,
            Ingredients =
            {
                new Ingredient(TechType.WiringKit, 1),
                new Ingredient(TechType.TitaniumIngot, 2),
                new Ingredient(TechType.Glass, 1),
                new Ingredient(TechType.PowerCell, 1)
            }
        };

#endif

        internal const string ExStorageKitClassID = "ExStorageKit_ASTS";

        public static string GetSaveFileDirectory()
        {
            return Path.Combine(SaveUtils.GetCurrentSaveDataDir(), ModName);
        }

        public static void OnSaveComplete()
        {
            _saveObject.StartCoroutine(SaveCoroutine());
        }

        private static IEnumerator SaveCoroutine()
        {
            while (SaveLoadManager.main != null && SaveLoadManager.main.isSaving)
            {
                yield return null;
            }
            GameObject.DestroyImmediate(_saveObject.gameObject);
            _saveObject = null;
        }

        public static bool IsSaving()
        {
            return _saveObject != null;
        }

        internal static string ConfigurationFile()
        {
            return Path.Combine(MODFOLDERLOCATION, "config.json");
        }

        internal static string GetAssetPath()
        {
            return Path.Combine(GetModPath(), "Assets");
        }

        private static string GetModPath()
        {
            return Path.Combine(GetQModsPath(), ModFolderName);
        }
        private static string GetQModsPath()
        {
            return Path.Combine(Environment.CurrentDirectory, "QMods");
        }

        #region ExStorageDepot
        public static void SaveExStorageDepot()
        {
            if (!IsSaving())
            {
                _saveObject = new GameObject().AddComponent<ModSaver>();

                ExStorageDepotSaveData newSaveData = new ExStorageDepotSaveData();

                var storages = GameObject.FindObjectsOfType<ExStorageDepotController>();

                QuickLogger.Debug($"Storages count {storages.Length}");

                foreach (var storage in storages)
                {
                    storage.Save(newSaveData);
                }

                _exStorageDepotSaveData = newSaveData;

                ModUtils.Save<ExStorageDepotSaveData>(_exStorageDepotSaveData, SaveDataFilename, GetSaveFileDirectory(), OnSaveComplete);
            }
        }

        public static void LoadExStorageDepotData()
        {
            QuickLogger.Info("Loading Save Data...");
            ModUtils.LoadSaveData<ExStorageDepotSaveData>(SaveDataFilename, GetSaveFileDirectory(), (data) =>
            {
                _exStorageDepotSaveData = data;
                QuickLogger.Info("Save Data Loaded");
                OnExStorageDepotLoaded?.Invoke(_exStorageDepotSaveData);
            });
        }

        public static ExStorageDepotSaveData GetExStorageDepotSaveData()
        {
            return _exStorageDepotSaveData ?? new ExStorageDepotSaveData();
        }

        public static ExStorageDepotSaveDataEntry GetExStorageDepotSaveData(string id)
        {
            LoadExStorageDepotData();

            var saveData = GetExStorageDepotSaveData();

            foreach (var entry in saveData.Entries)
            {
                if (entry.Id == id)
                {
                    return entry;
                }
            }

            return new ExStorageDepotSaveDataEntry() { Id = id };
        }
        #endregion

        private static void CreateModConfiguration()
        {
            var config = new Config();

            var saveDataJson = JsonConvert.SerializeObject(config, Formatting.Indented);

            File.WriteAllText(ConfigurationFile(), saveDataJson);
        }

        private static Config LoadConfigurationData()
        {
            // == Load Configuration == //
            string configJson = File.ReadAllText(ConfigurationFile().Trim());

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;

            // == LoadData == //
            return JsonConvert.DeserializeObject<Config>(configJson, settings);
        }

        internal static Config LoadConfiguration()
        {
            if (!IsConfigAvailable())
            {
                CreateModConfiguration();
            }

            return LoadConfigurationData();
        }

        internal static bool IsConfigAvailable()
        {
            return File.Exists(ConfigurationFile());
        }

        public static TechType GetDSSServerTechType()
        {
            if (_dSSServerTechType == TechType.None)
            {
                _dSSServerTechType = "DSSServer".ToTechType();
            }

            return _dSSServerTechType;
        }
    }
}
