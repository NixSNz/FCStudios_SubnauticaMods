﻿using System;
using System.IO;
using FCS_AlterraHub.Enumerators;
using FCS_AlterraHub.Registration;
using FCS_AlterraHub.Spawnables;
using FCS_StorageSolutions.Configuration;
using FCS_StorageSolutions.Mods.AlterraStorage.Buildable;
using FCS_StorageSolutions.Mods.DataStorageSolutions.Mono.Antenna;
using FCSCommon.Extensions;
using FCSCommon.Helpers;
using FCSCommon.Utilities;
using SMLHelper.V2.Crafting;
using UnityEngine;

namespace FCS_StorageSolutions.Mods.DataStorageSolutions.Buildable
{
    internal class DSSAntennaPatch : SMLHelper.V2.Assets.Buildable
    {
        public override TechGroup GroupForPDA => TechGroup.ExteriorModules;
        public override TechCategory CategoryForPDA => TechCategory.ExteriorModule;
        public override string AssetsFolder => Mod.GetAssetPath();

        public DSSAntennaPatch() : base(Mod.DSSAntennaClassName, Mod.DSSAntennaFriendlyName, Mod.DSSAntennaDescription)
        {
            OnFinishedPatching += () =>
            {
                var dssAntennaKit = new FCSKit(Mod.DSSAntennaKitClassID, FriendlyName, Path.Combine(AssetsFolder, $"{ClassID}.png"));
                dssAntennaKit.Patch();
                FCSAlterraHubService.PublicAPI.CreateStoreEntry(TechType, Mod.DSSAntennaKitClassID.ToTechType(), 210000, StoreCategory.Storage);
                //    FCSAlterraHubService.PublicAPI.RegisterEncyclopediaEntry(TechType, new List<FcsEntryData>
                //    {
                //        new FcsEntryData
                //        {
                //            key = "HydroHarvester",
                //            unlocked = true,
                //            path = "fcs",
                //            timeCapsule = false,
                //            nodes = new []{ "fcs"},
                //            Description = "The hydroponic Harvester .....",
                //            Title = "Hydroponic Harvester",
                //            Verbose = true
                //        },
                //        new FcsEntryData
                //        {
                //            key = "HydroHarvester1",
                //            unlocked = true,
                //            path = "fcs",
                //            timeCapsule = false,
                //            nodes = new []{ "fcs"},
                //            Description = "The hydroponic Harvester .....",
                //            Title = "Hydroponic Harvester 1",
                //            Verbose = false
                //        }
                //    });

            };
        }

        public override GameObject GetGameObject()
        {
            try
            {
                var prefab = GameObject.Instantiate(ModelPrefab.DSSAntennaPrefab);

                var center = new Vector3(0f, 2.308932f, -0.06670582f);
                var size = new Vector3(2.882609f, 4.29576f, 2.970935f);


                GameObjectHelpers.AddConstructableBounds(prefab, size, center);

                var model = prefab.FindChild("model");

                //========== Allows the building animation and material colors ==========// 
                Shader shader = Shader.Find("MarmosetUBER");
                Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
                SkyApplier skyApplier = prefab.EnsureComponent<SkyApplier>();
                skyApplier.renderers = renderers;
                skyApplier.anchorSky = Skies.Auto;
                //========== Allows the building animation and material colors ==========// 

                // Add constructible
                var constructable = prefab.AddComponent<Constructable>();

                constructable.allowedOutside = true;
                constructable.allowedInBase = false;
                constructable.allowedOnGround = true;
                constructable.allowedOnWall = false;
                constructable.rotationEnabled = true;
                constructable.allowedOnCeiling = false;
                constructable.allowedInSub = false;
                constructable.allowedOnConstructables = false;
                constructable.model = model;
                constructable.techType = TechType;

                PrefabIdentifier prefabID = prefab.AddComponent<PrefabIdentifier>();
                prefabID.ClassId = ClassID;

                var lw = prefab.AddComponent<LargeWorldEntity>();
                lw.cellLevel = LargeWorldEntity.CellLevel.Global;

                prefab.AddComponent<TechTag>().type = TechType;
                prefab.AddComponent<DSSAntennaController>();
                return prefab;
            }
            catch (Exception e)
            {
                QuickLogger.Error(e.Message);
            }

            return null;
        }


#if SUBNAUTICA
        protected override TechData GetBlueprintRecipe()
        {
            return Mod.DSSAntennaIngredients;
        }
#elif BELOWZERO
        protected override RecipeData GetBlueprintRecipe()
        {
            return Mod.DSSAntennaIngredients;
        }
#endif
    }
}