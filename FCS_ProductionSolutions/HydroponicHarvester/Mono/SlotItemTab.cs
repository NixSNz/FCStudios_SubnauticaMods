﻿using System;
using FCS_ProductionSolutions.Configuration;
using FCS_ProductionSolutions.HydroponicHarvester.Models;
using FCSCommon.Components;
using FCSCommon.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace FCS_ProductionSolutions.HydroponicHarvester.Mono
{
    internal class SlotItemTab : InterfaceButton
    {
        private uGUI_Icon _icon;
        private TechType _iconTechType;
        internal PlantSlot Slot;
        private Text _amount;
        private bool Initialized => _icon != null;

        internal void Initialize(DisplayManager display, Action<string,object> onButtonClicked,int slotIndex)
        {
            if (Initialized) return;

            BtnName = "SlotButton";

            Slot = display._mono.GrowBedManager.GetSlot(slotIndex);
            _icon = gameObject.FindChild("Icon").AddComponent<uGUI_Icon>();
            _icon.sprite = SpriteManager.defaultSprite;
            
            var addDnaBtn = GameObjectHelpers.FindGameObject(gameObject, "AddDNABTN").AddComponent<InterfaceButton>();
            addDnaBtn.STARTING_COLOR = Color.gray;
            addDnaBtn.HOVER_COLOR = Color.white;
            addDnaBtn.OnButtonClick += (s, o) => { display.OpenDnaSamplesPage(this); };
            
            var removeDnaBtn = GameObjectHelpers.FindGameObject(gameObject, "RemoveDNABTN").AddComponent<InterfaceButton>();
            removeDnaBtn.STARTING_COLOR = Color.gray;
            removeDnaBtn.HOVER_COLOR = Color.white;
            removeDnaBtn.OnButtonClick += (s, o) =>
            {
                var result = Slot.TryClear();
                if (result)
                {
                    Clear();
                }
            };

            _amount = InterfaceHelpers.FindGameObject(gameObject, "Amount").GetComponent<Text>();
            UpdateCount();

            HOVER_COLOR = Color.white;
            STARTING_COLOR = new Color(.5f,.5f,.5f);
            OnButtonClick += onButtonClicked;

            Slot.TrackTab(this);
        }

        internal void SetIcon(TechType techType)
        {
            if (Mod.IsHydroponicKnownTech(techType, out var data))
            {
                _iconTechType = techType;
                _icon.sprite = SpriteManager.Get(data.PickType);
                Slot.GrowBedManager.AddSample(techType, Slot.Id);
                UpdateCount();
                Tag = new SlotData(techType, Slot.Id);
            }
            else
            {
                _icon.sprite = SpriteManager.Get(TechType.None);
            }
        }

        internal void UpdateCount()
        {
            _amount.text = $"{Slot.GetCount()}/{Slot.GetMaxCapacity()}";
        }

        internal void Clear()
        {
            _icon.sprite = SpriteManager.Get(TechType.None);
            SetIcon(TechType.None);
        }
    }
}