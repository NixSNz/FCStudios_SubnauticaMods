﻿using FCS_DeepDriller.Configuration;
using FCS_DeepDriller.Display;
using FCSCommon.Enums;
using FCSCommon.Helpers;
using FCSCommon.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace FCS_DeepDriller.Mono.Handlers
{
    internal class FCSDeepDrillerDisplay : AIDisplay
    {
        private FCSDeepDrillerController _mono;

        internal void Setup(FCSDeepDrillerController mono)
        {
            _mono = mono;

            if (!FindAllComponents())
            {
                QuickLogger.Error("Unable to find all components");
            }
        }

        public override void ClearPage()
        {
            throw new NotImplementedException();
        }

        public override void OnButtonClick(string btnName, object tag)
        {
            throw new NotImplementedException();
        }

        public override void ItemModified<T>(T item)
        {
            throw new NotImplementedException();
        }

        public override bool FindAllComponents()
        {
            QuickLogger.Debug("Find All Components");

            #region Canvas

            var canvasGameObject = gameObject.GetComponentInChildren<Canvas>()?.gameObject;

            if (canvasGameObject == null)
            {
                QuickLogger.Error("Canvas not found.");
                return false;
            }

            #endregion

            #region Power Button

            var powerBTN = canvasGameObject.FindChild("PowerBTN")?.gameObject;

            if (powerBTN == null)
            {
                QuickLogger.Error("Power Button not found.");
                return false;
            }

            var powerBtn = powerBTN.AddComponent<InterfaceButton>();
            powerBtn.OnButtonClick = OnButtonClick;
            powerBtn.BtnName = "PowerBTN";
            powerBtn.ButtonMode = InterfaceButtonMode.Background;
            powerBtn.TextLineOne = $"Toggle {Mod.ModFriendlyName} Power";
            powerBtn.Tag = this;
            #endregion

            #region Open Storage Button

            var openStorageBTN = canvasGameObject.FindChild("Open_BTN")?.gameObject;

            if (openStorageBTN == null)
            {
                QuickLogger.Error("Open Storage Button not found.");
                return false;
            }

            var openStorageBtn = openStorageBTN.AddComponent<InterfaceButton>();
            openStorageBtn.OnButtonClick = OnButtonClick;
            openStorageBtn.BtnName = "Open_BTN";
            openStorageBtn.ButtonMode = InterfaceButtonMode.Background;
            openStorageBtn.TextLineOne = $"Open {Mod.ModFriendlyName} Storage";
            openStorageBtn.Tag = this;

            #endregion

            #region Open Modules Button

            var openModuleDoor = canvasGameObject.FindChild("moduleDoor")?.gameObject;

            if (openModuleDoor == null)
            {
                QuickLogger.Error("Open module door not found.");
                return false;
            }

            var moduleDoor = openModuleDoor.AddComponent<InterfaceButton>();
            moduleDoor.OnButtonClick = OnButtonClick;
            moduleDoor.BtnName = "Module_Door";
            moduleDoor.ButtonMode = InterfaceButtonMode.Background;
            moduleDoor.TextLineOne = $"Open {Mod.ModFriendlyName} Modulars";
            moduleDoor.Tag = this;

            #endregion

            #region Battery

            _batteryMeter = _canvasGameObject.FindChild("Battery")?.gameObject;

            if (_batteryMeter == null)
            {
                QuickLogger.Error("Open Storage Button not found.");
                return false;
            }

            #endregion


            return true;
        }

        public override IEnumerator PowerOff()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator PowerOn()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator ShutDown()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator CompleteSetup()
        {
            throw new NotImplementedException();
        }

        public override void DrawPage(int page)
        {
            throw new NotImplementedException();
        }
    }
}