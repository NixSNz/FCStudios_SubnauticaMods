﻿using FCSPowerStorage.Configuration;
using FCSPowerStorage.Helpers;
using FCSPowerStorage.Utilities.Enums;
using FCSSubnauticaCore.Objects;
using FCSTerminal.Logging;
using Oculus.Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FCSPowerStorage.Model.Components
{
    /**
* Component that contains the controller to the view. Majority of the view is set up already on the prefab inside of the unity editor.
* Handles such things as the paginator, drawing all the items that are on the "current page"
* Handles the idle screen saver.
* Handles the welcome animations.
*/
    public class FCSPowerStorageDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private float _timeSinceLastInteraction;

        #region Public Properties
        public readonly float MAX_INTERACTION_DISTANCE = 2.5f;
        public Animator Animator { get; private set; }
        private GameObject _welcomeScreen;
        private GameObject _settingsScreen;
        private GameObject _batteryMonitorPage;
        private GameObject _powerOffPage;
        public GameObject NavigationDock { get; private set; }
        private readonly float WELCOME_ANIMATION_TIME = 5.0f;
        private readonly float BOOTING_ANIMATION_TIME = 15.0f;
        public readonly float BATTERY_MONITOR_PAGE_ANIMATION_TIME = 3.0f;
        public readonly float SETTINGS_PAGE_ANIMATION_TIME = 1.783f;
        public static readonly float MAX_INTERACTION_IDLE_PAGE_DISTANCE = 5f;
        private static readonly float IDLE_TIME = 20f;
        private static readonly float IDLE_TIME_RANDOMNESS_LOW_BOUND = 1f;
        private static readonly float IDLE_TIME_RANDOMNESS_HIGH_BOUND = 10f;
        private bool _isHoveredOutOfRange;
        private bool _isHovered;
        private bool _isIdle;
        private float _idlePeriodLength = IDLE_TIME;
        private GameObject _background;
        private GameObject _bootingPage;
        private GameObject _blackout;
        private GameObject _homeButton;
        private GameObject _powerButton;
        private GameObject _settingButton;
        public GameObject _batteryMonitorAmountLBL { get; set; }
        private bool _inPowerOffMode;
        public GameObject TrickleModeCheckBox { get; set; }
        private GameObject _colorPicker;
        private GameObject _colorPickerPage;
        private List<SerializableColor> _serializedColors;
        private readonly int ITEMS_PER_PAGE = 28;
        private int _maxPage=1;
        private int _currentPage=1;
        private Text _pageCounterText;
        private GameObject _previousPageGameObject;
        private GameObject _nextPageGameObject;
        private GameObject COLOR_ITEM_PREFAB;
        private GameObject _pageCounter;
        public GameObject ChargeModeCheckBox { get; set; }
        public Transform BatteryStatus1Bar { get; set; }
        public Transform BatteryStatus1Percentage { get; set; }
        public Transform BatteryStatus2Bar { get; set; }
        public Transform BatteryStatus2Percentage { get; set; }
        public Transform BatteryStatus3Bar { get; set; }
        public Transform BatteryStatus3Percentage { get; set; }
        public Transform BatteryStatus4Bar { get; set; }
        public Transform BatteryStatus4Percentage { get; set; }
        public Transform BatteryStatus5Bar { get; set; }
        public Transform BatteryStatus5Percentage { get; set; }
        public Transform BatteryStatus6Bar { get; set; }
        public Transform BatteryStatus6Percentage { get; set; }


        public CustomBatteryController CustomBatteryController { get; private set; }

        public GameObject CanvasGameObject { get; set; }
        public static PowerToggleStates PowerToggleState { get; set; }

        #endregion

        public void Update()
        {
            UpdateBatteryMoniter();

            if (_inPowerOffMode)
            {
                _timeSinceLastInteraction = 0;
            }

            // === Update Battery Amount == //

            if (CustomBatteryController.ChargeMode == PowerToggleStates.TrickleMode)
            {
                _batteryMonitorAmountLBL.GetComponent<Text>().text = $"({Convert.ToInt32(CustomBatteryController.Charge)}/{Information.BatteryConfiguration.Capacity})";
            }
            else
            {
                _batteryMonitorAmountLBL.GetComponent<Text>().text = $"({Convert.ToInt32(CustomBatteryController.StoredPower)}/{Information.BatteryConfiguration.Capacity})";
            }

            // === Update Battery Amount == //

            //UpdatePowerState();

            if (_isIdle == false && _timeSinceLastInteraction < _idlePeriodLength)
            {
                _timeSinceLastInteraction += Time.deltaTime;
            }

            //Log.Info($"IsIdle = {_isIdle} Time Since Last Interaction{_timeSinceLastInteraction} Idle Period Length{_idlePeriodLength}");

            if (_isIdle == false && _timeSinceLastInteraction >= _idlePeriodLength)
            {
                EnterIdleScreen();
            }

            if (_isHovered == false && _isHoveredOutOfRange && InIdleInteractionRange())
            {
                _isHovered = true;
                ExitIdleScreen();
            }

            if (_isHovered)
            {
                ResetIdleTimer();
            }
        }

        //private void UpdatePowerState()
        //{
        //    Log.Info($"PowerState : {CustomBatteryController.PowerState} | Has Died {CustomBatteryController.HasDied}");
        //    if (CustomBatteryController.PowerState && CustomBatteryController.HasDied)
        //    {
        //        StartCoroutine(CompleteSetup());
        //    }
        //    else if (!CustomBatteryController.PowerState)
        //    {
        //        EnterPowerOffScreen();
        //    }
        //}

        public void EnterPowerOffScreen()
        {
            //ResetScreen("black");
            //_powerOffPage.SetActive(true);
            _homeButton.SetActive(false);
            _settingButton.SetActive(false);
            //NavigationDock.SetActive(true);
            _inPowerOffMode = true;

        }

        public void ResetNavigationBar()
        {
            _homeButton.SetActive(true);
            _settingButton.SetActive(true);
            _powerButton.SetActive(true);
            _homeButton.GetComponent<Image>().color = Color.white;
            _settingButton.GetComponent<Image>().color = Color.white;
            _powerButton.GetComponent<Image>().color = Color.white;
        }

        private void UpdateBatteryMoniter()
        {
            //Log.Info(CustomBatteryController.HasBreakerTripped.ToString());
            if (!CustomBatteryController.HasBreakerTripped)
            {
                //Get the current total percentage
                const float seg = 333.3333333333333f;
                const float seg2 = seg * 2;
                const float seg3 = seg * 3;
                const float seg4 = seg * 4;
                const float seg5 = seg * 5;
                const float seg6 = seg * 6;
                int valueWhole;
                float value;

                if (CustomBatteryController.ChargeMode == PowerToggleStates.TrickleMode)
                {
                    value = CustomBatteryController.Charge;
                    valueWhole = Convert.ToInt32(value);
                }
                else
                {
                    value = CustomBatteryController.StoredPower;
                    valueWhole = Convert.ToInt32(value);
                }



                // == Battery 1 == //s
                if (value >= 0 && valueWhole <= seg)
                {
                    SetPercentage(BatteryStatus1Percentage, BatteryStatus1Bar, value, seg, 0);
                }
                // == Battery 2 == //
                else if (value >= seg && valueWhole <= seg2)
                {
                    SetPercentage(BatteryStatus2Percentage, BatteryStatus2Bar, value, seg2, seg);
                }
                // == Battery 3 == //
                else if (value >= seg2 && valueWhole <= seg3)
                {
                    SetPercentage(BatteryStatus3Percentage, BatteryStatus3Bar, value, seg3, seg2);
                }
                // == Battery 4 == //
                else if (value >= seg3 && valueWhole <= seg4)
                {
                    SetPercentage(BatteryStatus4Percentage, BatteryStatus4Bar, value, seg4, seg3);
                }
                // == Battery 5 == //
                else if (value >= seg4 && valueWhole <= seg5)
                {
                    SetPercentage(BatteryStatus5Percentage, BatteryStatus5Bar, value, seg5, seg4);
                }
                // == Battery 6 == //
                else if (value >= seg5 && valueWhole <= 2000)
                {
                    SetPercentage(BatteryStatus6Percentage, BatteryStatus6Bar, value, seg6, seg5);
                }
            }
        }

        private void SetPercentage(Transform percentTrans, Transform percentBar, float value, float max, float min)
        {
            var percent = ((value - min) * 100) / (max - min);

            var text = percentTrans.GetComponent<Text>();
            text.text = $"{Convert.ToInt32(percent)}%";

            var bar = percentBar.GetComponent<Image>();
            bar.fillAmount = (float)Math.Round(percent / 100, 2);

            //Log.Info($"Fill Amount: {bar.fillAmount}/1");
        }

        public void Setup(CustomBatteryController cBcController)
        {
            COLOR_ITEM_PREFAB = AssetHelper.Asset.LoadAsset<GameObject>("ColorItem");
            if (cBcController.IsBeingDeleted) return;
            CustomBatteryController = cBcController;

            if (FindAllComponents() == false)
            {
                TurnDisplayOff();
                return;
            }

            UpdateChargeMode();
            StartCoroutine(CompleteSetup());

            _serializedColors = JsonConvert.DeserializeObject<List<SerializableColor>>(File.ReadAllText(Path.Combine(Information.ASSETSFOLDER, "colors.json")));

            //LoadColorPicker(serializedColors);
        }

        public void TurnDisplayOff()
        {
            StopCoroutine(CompleteSetup());
            _blackout.SetActive(true);
        }

        public IEnumerator CompleteSetup()
        {
            //ResetScreen("none");

            if (!CustomBatteryController.HasBreakerTripped)
            {
                _inPowerOffMode = false;

                Animator.enabled = true;


                yield return new WaitForEndOfFrame();
                if (CustomBatteryController.IsBeingDeleted) yield break;

                //_bootingPage.SetActive(true);
                var bootMode = Animator.GetBool("BootMode");
                var reboot = Animator.GetBool("Reboot");
                var powerOff = Animator.GetBool("PowerOff");
                var powerOn = Animator.GetBool("PowerOn");
                var settingsPage = Animator.GetBool("SettingsPage");
                var batteryMonitorTrans = Animator.GetBool("BatteryMonitorTrans");
                var loadWelcome = Animator.GetBool("LoadWelcome");



                Animator.SetBool("PowerOff", false);


                Log.Info($"BootMode State: {bootMode}");

                Animator.SetBool("BootMode", !bootMode);


                if (CustomBatteryController.IsBeingDeleted) yield break;
                yield return new WaitForSeconds(BOOTING_ANIMATION_TIME);
                if (CustomBatteryController.IsBeingDeleted) yield break;

                Animator.SetBool("Reboot", false);
                Animator.SetBool("LoadWelcome", !loadWelcome);


                if (CustomBatteryController.IsBeingDeleted) yield break;
                yield return new WaitForSeconds(WELCOME_ANIMATION_TIME);
                yield return new WaitForSeconds(BATTERY_MONITOR_PAGE_ANIMATION_TIME);
                if (CustomBatteryController.IsBeingDeleted) yield break;

                //NavigationDock.SetActive(true);

                Animator.SetBool("Reboot", false);
                Animator.SetBool("PowerOn", false);
                DrawPage(1);
            }
            else
            {
                yield return new WaitForEndOfFrame();
                if (CustomBatteryController.IsBeingDeleted) yield break;
                Animator.SetBool("PowerOff", true);
                yield return new WaitForSeconds(3);
                Animator.SetBool("PowerOff", false);
            }
            //Animator.SetBool("BootMode", bootMode);


            //Animator.enabled = false;

            //_blackout.SetActive(false);
            //_bootingPage.SetActive(false);

            //Animator.Play("WelcomePage", -1, 0f);
            //_background.SetActive(true);

            //if (CustomBatteryController.IsBeingDeleted) yield break;
            //yield return new WaitForSeconds(WELCOME_ANIMATION_TIME);
            //if (CustomBatteryController.IsBeingDeleted) yield break;
            //_welcomeScreen.SetActive(false);


            //_batteryMonitorPage.SetActive(true);
            //Animator.Play("BatteryMonitorPage", -1, 0f);

            //if (CustomBatteryController.IsBeingDeleted) yield break;
            //yield return new WaitForSeconds(BATTERY_MONITOR_PAGE_ANIMATION_TIME);
            //if (CustomBatteryController.IsBeingDeleted) yield break;

            //if (CustomBatteryController.IsBeingDeleted) yield break;
            //NavigationDock.SetActive(true);

            //if (CustomBatteryController.IsBeingDeleted) yield break;

            //_batteryMonitorPage.SetActive(true);

            //Animator.enabled = false;
        }

        public void ResetScreen(string screen)
        {
            Log.Info("In Reset Screen");
            _welcomeScreen.SetActive(false);
            _batteryMonitorPage.SetActive(false);
            _settingsScreen.SetActive(false);
            _powerOffPage.SetActive(false);
            //NavigationDock.SetActive(false);
            _bootingPage.SetActive(false);
            _blackout.SetActive(false);
            _background.SetActive(false);

            if (screen.Equals("black"))
            {
                _blackout.SetActive(true);
            }
            else if (screen.Equals("blue"))
            {
                _background.SetActive(true);

            }
        }

        public void ResetAnimation()
        {
            Log.Info("Reset Animation");
            Animator.SetBool("BootMode", false);
            Animator.SetBool("Reboot", false);
            Animator.SetBool("PowerOff", false);
            Animator.SetBool("PowerOn", false);
            Animator.SetBool("BatteryMonitorTrans", false);
            Animator.SetBool("SettingsPageTrans", false);
            Animator.SetBool("LoadWelcome", false);
            PrintAnimationValues();
        }

        public void PrintAnimationValues()
        {
            Log.Info($"// ==================================================================== //");

            Log.Info($"BootMode = {Animator.GetBool("BootMode")}");
            Log.Info($"Reboot = {Animator.GetBool("Reboot")}");
            Log.Info($"PowerOff = {Animator.GetBool("PowerOff")}");
            Log.Info($"PowerOn = {Animator.GetBool("PowerOn")}");
            Log.Info($"SettingsPage = {Animator.GetBool("SettingsPage")}");
            Log.Info($"BatteryMonitorTrans = {Animator.GetBool("BatteryMonitorTrans")}");
            Log.Info($"LoadWelcome = {Animator.GetBool("LoadWelcome")}");

            Log.Info($"// ==================================================================== //");

        }

        private bool FindAllComponents()
        {
            CanvasGameObject = gameObject.GetComponentInChildren<Canvas>()?.gameObject;

            if (CanvasGameObject == null)
            {
                Log.Error("Canvas not found.");
                return false;
            }

            Animator = CanvasGameObject.GetComponent<Animator>();

            if (Animator == null)
            {
                Log.Error("Animator not found.");
                return false;
            }

            GameObject screenHolder = CanvasGameObject.transform.Find("Screens")?.gameObject;

            if (screenHolder == null)
            {
                Log.Error("Screen Holder GameObject not found.");
                return false;
            }


            _welcomeScreen = screenHolder.FindChild("WelcomePage")?.gameObject;
            if (_welcomeScreen == null)
            {
                Log.Error("Screen: WelcomeScreen not found.");
                return false;
            }

            _settingsScreen = screenHolder.FindChild("SettingsPage")?.gameObject;
            if (_settingsScreen == null)
            {
                Log.Error("Screen: SettingsPage not found.");
                return false;
            }

            _batteryMonitorPage = screenHolder.FindChild("BatteryMonitorPage")?.gameObject;
            if (_batteryMonitorPage == null)
            {
                Log.Error("Screen: BatteryMonitorPage not found.");
                return false;
            }

            _batteryMonitorAmountLBL = screenHolder.FindChild("BatteryMonitorPage").FindChild("Battery_Monitor_Amount_LBL")?.gameObject;
            if (_batteryMonitorAmountLBL == null)
            {
                Log.Error("Screen: Battery_Monitor_Amount_LBL not found.");
                return false;
            }

            _powerOffPage = screenHolder.FindChild("PowerOffPage")?.gameObject;
            if (_powerOffPage == null)
            {
                Log.Error("Screen: PowerOffPage not found.");
                return false;
            }

            NavigationDock = CanvasGameObject.transform.Find("Navigation_Dock")?.gameObject;
            if (NavigationDock == null)
            {
                Log.Error("Dock: Navigation_Dock not found.");
                return false;
            }

            _background = CanvasGameObject.transform.Find("Background")?.gameObject;
            if (_background == null)
            {
                Log.Error("Screen: Background not found.");
                return false;
            }

            _bootingPage = screenHolder.FindChild("BootingPage")?.gameObject;
            if (_bootingPage == null)
            {
                Log.Error("Screen: BootingPage not found.");
                return false;
            }

            _blackout = CanvasGameObject.transform.Find("BlackOut")?.gameObject;
            if (_blackout == null)
            {
                Log.Error("Screen: BlackOut not found.");
                return false;
            }

            _homeButton = NavigationDock.transform.Find("Home_BTN")?.gameObject;
            if (_homeButton == null)
            {
                Log.Error("Dock: Home_BTN not found.");
                return false;
            }
            else
            {
                InterfaceButton home = _homeButton.AddComponent<InterfaceButton>();
                home.FcsPowerStorageDisplay = this;
                home.ChangePage = _batteryMonitorPage;
            }

            _powerButton = NavigationDock.transform.Find("Power_BTN")?.gameObject;
            if (_powerButton == null)
            {
                Log.Error("Dock: Power_BTN not found.");
                return false;
            }
            else
            {
                InterfaceButton power = _powerButton.AddComponent<InterfaceButton>();
                power.FcsPowerStorageDisplay = this;
                power.ChangePage = _powerOffPage;
            }

            _settingButton = NavigationDock.transform.Find("Settings_BTN")?.gameObject;
            if (_settingButton == null)
            {
                Log.Error("Dock: Settings_BTN not found.");
                return false;
            }
            else
            {
                InterfaceButton settings = _settingButton.AddComponent<InterfaceButton>();
                settings.FcsPowerStorageDisplay = this;
                settings.ChangePage = _settingsScreen;
            }

            if (_blackout == null)
            {
                Log.Error("Screen: BlackOut not found.");
                return false;
            }

            foreach (Transform progessBar in screenHolder.FindChild("BatteryMonitorPage").FindChild("Grid").transform)
            {
                if (progessBar == null)
                {
                    Log.Error($"Screen: A Progress bar was not found.");
                    return false;
                }
                else
                {
                    switch (progessBar.name)
                    {
                        case "Battery_Status_1":
                            BatteryStatus1Bar = progessBar.Find("ProgressBar");
                            BatteryStatus1Percentage = progessBar.Find("Percentage");
                            break;
                        case "Battery_Status_2":
                            BatteryStatus2Bar = progessBar.Find("ProgressBar");
                            BatteryStatus2Percentage = progessBar.Find("Percentage");
                            break;
                        case "Battery_Status_3":
                            BatteryStatus3Bar = progessBar.Find("ProgressBar");
                            BatteryStatus3Percentage = progessBar.Find("Percentage");
                            break;
                        case "Battery_Status_4":
                            BatteryStatus4Bar = progessBar.Find("ProgressBar");
                            BatteryStatus4Percentage = progessBar.Find("Percentage");
                            break;
                        case "Battery_Status_5":
                            BatteryStatus5Bar = progessBar.Find("ProgressBar");
                            BatteryStatus5Percentage = progessBar.Find("Percentage");
                            break;
                        case "Battery_Status_6":
                            BatteryStatus6Bar = progessBar.Find("ProgressBar");
                            BatteryStatus6Percentage = progessBar.Find("Percentage");
                            break;
                    }
                }
            }

            TrickleModeBTN = _settingsScreen.FindChild("Trickle_Mode")?.gameObject;
            if (TrickleModeBTN == null)
            {
                Log.Error("Screen: Trickle_Mode not found.");
                return false;
            }
            else
            {
                InterfaceButton settings = TrickleModeBTN.AddComponent<InterfaceButton>();
                settings.FcsPowerStorageDisplay = this;
                settings.IsToggle = true;
                settings.ToggleBtnName = "Trickle_Mode";
            }

            TrickleModeCheckBox = TrickleModeBTN.FindChild("Background").FindChild("Checkmark")?.gameObject;
            if (TrickleModeCheckBox == null)
            {
                Log.Error("Screen: Trickle_Mode =>Checkmark not found.");
                return false;
            }



            ChargeModeBTN = _settingsScreen.FindChild("Charge_Mode")?.gameObject;
            if (ChargeModeBTN == null)
            {
                Log.Error("Screen: Charge_Mode not found.");
                return false;
            }
            else
            {
                InterfaceButton settings = ChargeModeBTN.AddComponent<InterfaceButton>();
                settings.FcsPowerStorageDisplay = this;
                settings.IsToggle = true;
                settings.ToggleBtnName = "Charge_Mode";
            }

            ChargeModeCheckBox = ChargeModeBTN.FindChild("Background").FindChild("Checkmark")?.gameObject;
            if (ChargeModeCheckBox == null)
            {
                Log.Error("Screen: Charge_Mode =>Checkmark not found.");
                return false;
            }


            _colorPicker = _settingsScreen.FindChild("Color_Picker")?.gameObject;
            if (_colorPicker == null)
            {
                Log.Error("Screen: _color_Picker not found.");
                return false;
            }
            else
            {
                InterfaceButton settings = _colorPicker.AddComponent<InterfaceButton>();
                settings.FcsPowerStorageDisplay = this;
                settings.IsToggle = true;
                settings.ToggleBtnName = "Color_Picker";
            }


            _colorPickerPage = screenHolder.FindChild("ColorPickerPage")?.gameObject;
            if (_colorPickerPage == null)
            {
                Log.Error("Screen: ColorPicker not found.");
                return false;
            }

            _previousPageGameObject = _colorPickerPage.FindChild("Back_Arrow_BTN")?.gameObject;
            if (_previousPageGameObject == null)
            {
                Log.Error("Screen: Back_Arrow_BTN not found.");
                return false;
            }
            else
            {
                var pb2 = _previousPageGameObject.AddComponent<PaginatorButton>();
                pb2.FcsPowerStorageDisplay = this;
                pb2.AmountToChangePageBy = -1;
            }

            _nextPageGameObject = _colorPickerPage.FindChild("Forward_Arrow_BTN")?.gameObject;
            if (_nextPageGameObject == null)
            {
                Log.Error("Screen: Forward_Arrow_BTN not found.");
                return false;
            }
            else
            {
                var pb2 = _nextPageGameObject.AddComponent<PaginatorButton>();
                pb2.FcsPowerStorageDisplay = this;
                pb2.AmountToChangePageBy = 1;
            }

            _pageCounter = _colorPickerPage.FindChild("Page_Number")?.gameObject;
            if (_pageCounter == null)
            {
                Log.Error("Screen: Page_Number not found.");
                return false;
            }


            _pageCounterText = _pageCounter.GetComponent<Text>();
            if (_pageCounterText == null)
            {
                Log.Error("Screen: _pageCounterText not found.");
                return false;
            }



            return true;
        }

        private void CalculateNewMaxPages()
        {
            _maxPage = Mathf.CeilToInt((_serializedColors.Count - 1) / ITEMS_PER_PAGE) + 1;
            if (_currentPage > _maxPage)
            {
                _currentPage = _maxPage;
            }
        }

        private void DrawPage(int page)
        {
            _currentPage = page;

            if (_currentPage <= 0)
            {
                _currentPage = 1;
            }
            else if (_currentPage > _maxPage)
            {
                _currentPage = _maxPage;
            }

            int startingPosition = (_currentPage - 1) * ITEMS_PER_PAGE;
            int endingPosition = startingPosition + ITEMS_PER_PAGE;

            //====================================================================//
            Log.Info($"//=================== StartPosition | {startingPosition} ============================== //");
            Log.Info($"//=================== StartPosition | {endingPosition} ============================== //");
            Log.Info($"//=================== SerializedColors Count| {_serializedColors.Count} ============================== //");
            //====================================================================//

            if (endingPosition > _serializedColors.Count)
            {
                endingPosition = _serializedColors.Count;
            }

            ClearPage();

            for (int i = startingPosition; i < endingPosition; i++)
            {
                var colorID = _serializedColors.ElementAt(i);
                LoadColorPicker(colorID);
            }

            UpdatePaginator();
        }

        private void ClearPage()
        {
            for (int i = 0; i < _colorPickerPage.FindChild("ColorPicker").transform.childCount; i++)
            {
                Destroy(_colorPickerPage.FindChild("ColorPicker").transform.GetChild(i).gameObject);
            }
        }

        private void UpdatePaginator()
        {
            CalculateNewMaxPages();
            _pageCounter.SetActive(_serializedColors.Count!=0);
            _pageCounterText.text = $"{_currentPage} / {_maxPage}";
            _previousPageGameObject.SetActive(_currentPage != 1);
            _nextPageGameObject.SetActive(_currentPage != _maxPage);
        }

        public GameObject TrickleModeBTN { get; set; }

        public GameObject ChargeModeBTN { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isIdle && InIdleInteractionRange())
            {
                ExitIdleScreen();
            }

            if (_isIdle == false)
            {
                ResetIdleTimer();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHoveredOutOfRange = true;
            if (InIdleInteractionRange())
            {
                _isHovered = true;
            }

            if (_isIdle && InIdleInteractionRange())
            {
                ExitIdleScreen();
            }

            if (_isIdle == false)
            {
                ResetIdleTimer();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHoveredOutOfRange = false;
            _isHovered = false;
            if (_isIdle && InIdleInteractionRange())
            {
                ExitIdleScreen();
            }

            if (_isIdle == false)
            {
                ResetIdleTimer();
            }
        }

        private void EnterIdleScreen()
        {
            Log.Info($"EnterIdleScreen");

            _isIdle = true;
            _settingsScreen.SetActive(false);
            ResetIdleTimer();

            Animator.SetBool("BootMode", true);
            //_batteryMonitorPage.SetActive(true);
            NavigationDock.SetActive(false);

            //if (CustomBatteryController.Charge.Equals(0.0f))
            //{
            //    _powerOffPage.SetActive(true);
            //    _batteryMonitorPage.SetActive(false);
            //    NavigationDock.SetActive(false);
            //}
            //else
            //{
            //    _powerOffPage.SetActive(false);

            //}

        }

        private void ExitIdleScreen()
        {
            Log.Info($"ExitIdleScreen");
            _settingsScreen.SetActive(false);


            _isIdle = false;
            ResetIdleTimer();
            Animator.SetBool("BootMode", true);
            NavigationDock.SetActive(true);

            //if (CustomBatteryController.Charge.Equals(0.0f))
            //{
            //    Log.Info($"ExitIdleScreen");

            //    _powerOffPage.SetActive(true);
            //    _batteryMonitorPage.SetActive(false);
            //    NavigationDock.SetActive(false);
            //}
            //else
            //{
            //    Log.Info($"ExitIdleScreen");

            //    _powerOffPage.SetActive(false);

            //}
            CalculateNewIdleTime();

        }

        private void CalculateNewIdleTime()
        {
            _idlePeriodLength = IDLE_TIME + Random.Range(IDLE_TIME_RANDOMNESS_LOW_BOUND, IDLE_TIME_RANDOMNESS_HIGH_BOUND);
            Log.Info($"CalculateNewIdleTime:  Idle Period Length{_idlePeriodLength}");

        }

        internal void ResetIdleTimer()
        {
            _timeSinceLastInteraction = 0f;
            //Log.Info($"ResetIdleTimer:  time Since Last Interaction{_timeSinceLastInteraction}");
        }

        private bool InIdleInteractionRange()
        {
            var result = Mathf.Abs(Vector3.Distance(gameObject.transform.position, Player.main.transform.position)) <= MAX_INTERACTION_IDLE_PAGE_DISTANCE;

            //Log.Info($"InIdleInteractionRange: {result}");
            return result;
        }

        public void ResetCoroutine()
        {
            StopCoroutine("CompleteSetup");
            StartCoroutine("CompleteSetup");
        }

        public void UpdateChargeMode()
        {
            if (CustomBatteryController.ChargeMode == PowerToggleStates.ChargeMode)
            {
                ChargeModeCheckBox.SetActive(true);
                TrickleModeCheckBox.SetActive(false);
            }
            else if (CustomBatteryController.ChargeMode == PowerToggleStates.TrickleMode)
            {
                ChargeModeCheckBox.SetActive(false);
                TrickleModeCheckBox.SetActive(true);
            }

        }

        private void LoadColorPicker(SerializableColor color)
        {

            //Log.Info($"// ==================== Adding Color {serializableColor.r}| {serializableColor.g} | {serializableColor.b} ================== // ");
            GameObject itemDisplay = Instantiate(COLOR_ITEM_PREFAB);
            itemDisplay.transform.SetParent(_colorPickerPage.FindChild("ColorPicker").transform, false);
            itemDisplay.GetComponentInChildren<Image>().color = color.ToColor();

            var itemButton = itemDisplay.AddComponent<ColorItemButton>();
            itemButton.FcsPowerStorageDisplay = this;
            itemButton.Color = color.ToColor();

            //Log.Info($"// ==================== Added Color {serializableColor.r}| {serializableColor.g} | {serializableColor.b} ================== // ");



        }

        /// <summary>
        /// Method that allows page change on the color picker page
        /// </summary>
        /// <param name="amount"></param>
        public void ChangePageBy(int amount)
        {
            DrawPage(_currentPage + amount);
        }
    }
}
