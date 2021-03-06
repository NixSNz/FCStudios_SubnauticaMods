﻿using FCS_AIMarineTurbine.Buildable;
using FCS_AIMarineTurbine.Patches;
using FCSCommon.Utilities;
using System;
using UnityEngine;

namespace FCS_AIMarineTurbine.Model
{
    internal sealed class AISolutionsData
    {
        private AISolutionsData()
        {

        }

        internal class BiomeItem
        {
            /// <summary>
            /// The speed of the turbine
            /// </summary>
            public float Speed { get; set; }
        }

        private static AISolutionsData _instance;

        internal static void PatchHelper()
        {
            ChangeRotation();
            uGUI_DepthCompassLateUpdate_Patcher.AddEventHandlerIfMissing(Update);
        }

        internal static AISolutionsData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AISolutionsData();
                }
                return _instance;
            }
        }

        private static void Update()
        {
            if (DayNightCycle.main == null) return;

            _passedTime += DayNightCycle.main.deltaTime;

            if (_passedTime >= AIJetStreamT242Buildable.JetStreamT242Config.RotationCycleInSec)
            {
                QuickLogger.Debug($"ChangeRotation");
                ChangeRotation();
                _passedTime = 0.0f;
            }
        }

        internal static Quaternion StartingRotation { get; set; }

        private static float _passedTime;

        public event Action<Quaternion> OnRotationChanged;

        private static void ChangeRotation()
        {
            var magNorth = -Input.compass.magneticHeading;
            StartingRotation = Quaternion.Euler(0, magNorth + RandomNumber.Between(-180, 180), 0);
            Instance.OnRotationChanged?.Invoke(StartingRotation);
        }

    }
}
