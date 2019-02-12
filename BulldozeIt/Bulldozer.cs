using ColossalFramework.UI;
using System;
using UnityEngine;

namespace BulldozeIt
{
    class Bulldozer : MonoBehaviour
    {
        private bool _initialized;

        private UISlicedSprite _tsBar;
        private UIMultiStateButton _bulldozerUndergroundToggle;
        private UISprite _happiness;
        private UITextureAtlas _textureAtlas;
        private UICheckBox _abandonedButton;
        private UISprite _abandonedClock;
        private UILabel _abandonedCounter;
        private UICheckBox _burnedDownButton;
        private UISprite _burnedDownClock;
        private UILabel _burnedDownCounter;
        private UICheckBox _collapsedButton;
        private UISprite _collapsedClock;
        private UILabel _collapsedCounter;
        private UICheckBox _floodedButton;
        private UISprite _floodedClock;
        private UILabel _floodedCounter;
        private UIButton _bulldozingStatistics;

        private void Awake()
        {
            try
            {
                if (_tsBar == null)
                {
                    _tsBar = GameObject.Find("TSBar").GetComponent<UISlicedSprite>();
                }

                if (_bulldozerUndergroundToggle == null)
                {
                    _bulldozerUndergroundToggle = GameObject.Find("BulldozerUndergroundToggle").GetComponent<UIMultiStateButton>();
                }

                if (_happiness == null)
                {
                    _happiness = GameObject.Find("Happiness").GetComponent<UISprite>();
                }

                _textureAtlas = LoadResources();

                CreateUI();
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:Awake -> Exception: " + e.Message);
            }
        }

        private void OnEnable()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:OnEnable -> Exception: " + e.Message);
            }
        }

        private void Start()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:Start -> Exception: " + e.Message);
            }
        }

        private void Update()
        {
            try
            {
                if (BulldozeManager.Instance.FinishedRunOnce)
                {
                    ResetRunOnce();
                }

                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    UpdateUI();

                    _initialized = true;
                    ModConfig.Instance.ConfigUpdated = false;
                }

                if (_bulldozerUndergroundToggle.isVisible)
                {
                    _abandonedButton.isVisible = true;
                    _burnedDownButton.isVisible = true;
                    _collapsedButton.isVisible = true;
                    _floodedButton.isVisible = true;

                    _abandonedClock.isVisible = BulldozeManager.Instance.AbandonedBuildingsRunOnce;
                    _burnedDownClock.isVisible = BulldozeManager.Instance.BurnedDownBuildingsRunOnce;
                    _collapsedClock.isVisible = BulldozeManager.Instance.CollapsedBuildingsRunOnce;
                    _floodedClock.isVisible = BulldozeManager.Instance.FloodedBuildingsRunOnce;

                    if (ModConfig.Instance.ShowCounters)
                    {
                        UpdateCounters();
                    }
                }
                else
                {
                    _abandonedButton.isVisible = false;
                    _burnedDownButton.isVisible = false;
                    _collapsedButton.isVisible = false;
                    _floodedButton.isVisible = false;

                    _abandonedClock.isVisible = false;
                    _burnedDownClock.isVisible = false;
                    _collapsedClock.isVisible = false;
                    _floodedClock.isVisible = false;
                }

                if (ModConfig.Instance.ShowStatistics)
                {
                    UpdateStatistics();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:Update -> Exception: " + e.Message);
            }
        }

        private void OnDisable()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:OnDisable -> Exception: " + e.Message);
            }
        }

        private void OnDestroy()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:OnDestroy -> Exception: " + e.Message);
            }
        }

        private UITextureAtlas LoadResources()
        {
            try
            {
                if (_textureAtlas == null)
                {
                    string[] spriteNames = new string[]
                    {
                        "Abandoned",
                        "BurnedDown",
                        "Collapsed",
                        "Flooded",
                        "AbandonedFocused",
                        "BurnedDownFocused",
                        "CollapsedFocused",
                        "FloodedFocused",
                        "Clock",
                        "BulldozerCounter"
                    };

                    _textureAtlas = ResourceLoader.CreateTextureAtlas("BulldozeItAtlas", spriteNames, "BulldozeIt.Icons.");

                    UITextureAtlas defaultAtlas = ResourceLoader.GetAtlas("Ingame");
                    Texture2D[] textures = new Texture2D[]
                    {
                        defaultAtlas["OptionBase"].texture,
                        defaultAtlas["OptionBaseFocused"].texture,
                        defaultAtlas["OptionBaseHovered"].texture,
                        defaultAtlas["OptionBasePressed"].texture,
                        defaultAtlas["OptionBaseDisabled"].texture
                    };

                    ResourceLoader.AddTexturesInAtlas(_textureAtlas, textures);
                }

                return _textureAtlas;
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:LoadResources -> Exception: " + e.Message);
                return null;
            }
        }

        private void CreateUI()
        {
            try
            {
                _abandonedButton = UIUtils.CreateButtonCheckBox(_tsBar, "BulldozeItAbandoned", _textureAtlas, "Abandoned", CreateTooltip("Abandoned"), ModConfig.Instance.AbandonedBuildings);
                _abandonedButton.relativePosition = new Vector3(_bulldozerUndergroundToggle.relativePosition.x - 160f, _bulldozerUndergroundToggle.relativePosition.y);
                _abandonedButton.eventCheckChanged += (component, value) =>
                {
                    if (value && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
                    {
                        BulldozeManager.Instance.AbandonedBuildingsRunOnce = true;
                        BulldozeManager.Instance.IgnoreMaxAtRunOnce = true;
                    }
                    else if (value && Input.GetKey(KeyCode.LeftShift))
                    {
                        BulldozeManager.Instance.AbandonedBuildingsRunOnce = true;
                        BulldozeManager.Instance.IgnoreMaxAtRunOnce = false;
                    }

                    ModConfig.Instance.AbandonedBuildings = value;
                    ModConfig.Instance.Save();
                };

                _abandonedClock = UIUtils.CreateClockSprite(_abandonedButton, "BulldozeItAbandonedClock", _textureAtlas);
                _abandonedClock.relativePosition = new Vector3(20f, 20f);

                _abandonedCounter = UIUtils.CreateCounterLabel(_abandonedButton, "BulldozeItAbandonedCounter", "0", _textureAtlas, "BulldozerCounter");
                _abandonedCounter.relativePosition = new Vector3(0f, -12f);

                _burnedDownButton = UIUtils.CreateButtonCheckBox(_tsBar, "BulldozeItBurnedDown", _textureAtlas, "BurnedDown", CreateTooltip("Burned Down"), ModConfig.Instance.BurnedDownBuildings);
                _burnedDownButton.relativePosition = new Vector3(_bulldozerUndergroundToggle.relativePosition.x - 120f, _bulldozerUndergroundToggle.relativePosition.y);
                _burnedDownButton.eventCheckChanged += (component, value) =>
                {
                    if (value && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
                    {
                        BulldozeManager.Instance.BurnedDownBuildingsRunOnce = true;
                        BulldozeManager.Instance.IgnoreMaxAtRunOnce = true;
                    }
                    else if (value && Input.GetKey(KeyCode.LeftShift))
                    {
                        BulldozeManager.Instance.BurnedDownBuildingsRunOnce = true;
                        BulldozeManager.Instance.IgnoreMaxAtRunOnce = false;
                    }

                    ModConfig.Instance.BurnedDownBuildings = value;
                    ModConfig.Instance.Save();
                };

                _burnedDownClock = UIUtils.CreateClockSprite(_burnedDownButton, "BulldozeItBurnedDownClock", _textureAtlas);
                _burnedDownClock.relativePosition = new Vector3(20f, 20f);

                _burnedDownCounter = UIUtils.CreateCounterLabel(_burnedDownButton, "BulldozeItBurnedDownCounter", "0", _textureAtlas, "BulldozerCounter");
                _burnedDownCounter.relativePosition = new Vector3(0f, -12f);

                _collapsedButton = UIUtils.CreateButtonCheckBox(_tsBar, "BulldozeItCollapsed", _textureAtlas, "Collapsed", CreateTooltip("Collapsed"), ModConfig.Instance.CollapsedBuildings);
                _collapsedButton.relativePosition = new Vector3(_bulldozerUndergroundToggle.relativePosition.x - 80f, _bulldozerUndergroundToggle.relativePosition.y);
                _collapsedButton.eventCheckChanged += (component, value) =>
                {
                    if (value && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
                    {
                        BulldozeManager.Instance.CollapsedBuildingsRunOnce = true;
                        BulldozeManager.Instance.IgnoreMaxAtRunOnce = true;
                    }
                    else if (value && Input.GetKey(KeyCode.LeftShift))
                    {
                        BulldozeManager.Instance.CollapsedBuildingsRunOnce = true;
                        BulldozeManager.Instance.IgnoreMaxAtRunOnce = false;
                    }

                    ModConfig.Instance.CollapsedBuildings = value;
                    ModConfig.Instance.Save();
                };

                _collapsedClock = UIUtils.CreateClockSprite(_collapsedButton, "BulldozeItCollapsedClock", _textureAtlas);
                _collapsedClock.relativePosition = new Vector3(20f, 20f);

                _collapsedCounter = UIUtils.CreateCounterLabel(_collapsedButton, "BulldozeItCollapsedCounter", "0", _textureAtlas, "BulldozerCounter");
                _collapsedCounter.relativePosition = new Vector3(0f, -12f);

                _floodedButton = UIUtils.CreateButtonCheckBox(_tsBar, "BulldozeItFlooded", _textureAtlas, "Flooded", CreateTooltip("Flooded"), ModConfig.Instance.FloodedBuildings);
                _floodedButton.relativePosition = new Vector3(_bulldozerUndergroundToggle.relativePosition.x - 40f, _bulldozerUndergroundToggle.relativePosition.y);
                _floodedButton.eventCheckChanged += (component, value) =>
                {
                    if (value && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
                    {
                        BulldozeManager.Instance.FloodedBuildingsRunOnce = true;
                        BulldozeManager.Instance.IgnoreMaxAtRunOnce = true;
                    }
                    else if (value && Input.GetKey(KeyCode.LeftShift))
                    {
                        BulldozeManager.Instance.FloodedBuildingsRunOnce = true;
                        BulldozeManager.Instance.IgnoreMaxAtRunOnce = false;
                    }

                    ModConfig.Instance.FloodedBuildings = value;
                    ModConfig.Instance.Save();
                };

                _floodedClock = UIUtils.CreateClockSprite(_floodedButton, "BulldozeItFloodedClock", _textureAtlas);
                _floodedClock.relativePosition = new Vector3(20f, 20f);

                _floodedCounter = UIUtils.CreateCounterLabel(_floodedButton, "BulldozeItFloodedCounter", "0", _textureAtlas, "BulldozerCounter");
                _floodedCounter.relativePosition = new Vector3(0f, -12f);

                _bulldozingStatistics = UIUtils.CreateInfoButton(_happiness, "BulldozeItStatistics", "ToolbarIconBulldozer", "Automatic bulldozed buildings");
                _bulldozingStatistics.size = new Vector2(100f, 26f);
                _bulldozingStatistics.relativePosition = new Vector3(31f, 0f);
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:CreateUI -> Exception: " + e.Message);
            }
        }

        private string CreateTooltip(string typeOfBuilding)
        {
            string tooltip = string.Empty;

            tooltip = "Toggle Automatic Bulldozing of " + typeOfBuilding + " Buildings.";
            tooltip += Environment.NewLine + Environment.NewLine;
            tooltip += "<color #50869a>Left Shift:</color> Disable again after one cycle.";
            tooltip += Environment.NewLine + Environment.NewLine;
            tooltip += "<color #50869a>Left Control + Left Shift:</color> Disable again after one cycle (no maximum buildings).";

            return tooltip;
        }

        private void UpdateUI()
        {
            try
            {
                UpdateAllButtonCheckBoxes();

                if (ModConfig.Instance.ShowCounters)
                {
                    _abandonedCounter.isVisible = true;
                    _burnedDownCounter.isVisible = true;
                    _collapsedCounter.isVisible = true;
                    _floodedCounter.isVisible = true;
                }
                else
                {
                    _abandonedCounter.isVisible = false;
                    _burnedDownCounter.isVisible = false;
                    _collapsedCounter.isVisible = false;
                    _floodedCounter.isVisible = false;
                }

                _bulldozingStatistics.isVisible = ModConfig.Instance.ShowStatistics ? true : false;
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:UpdateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateAllButtonCheckBoxes()
        {
            try
            {
                UpdateButtonCheckBox(_abandonedButton, "Abandoned", ModConfig.Instance.AbandonedBuildings);
                UpdateButtonCheckBox(_burnedDownButton, "BurnedDown", ModConfig.Instance.BurnedDownBuildings);
                UpdateButtonCheckBox(_collapsedButton, "Collapsed", ModConfig.Instance.CollapsedBuildings);
                UpdateButtonCheckBox(_floodedButton, "Flooded", ModConfig.Instance.FloodedBuildings);
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:UpdateAllButtonCheckBoxes -> Exception: " + e.Message);
            }
        }

        private void UpdateButtonCheckBox(UICheckBox checkBox, string spriteName, bool state)
        {
            try
            {
                UIButton button;

                if (state)
                {
                    checkBox.isChecked = true;
                    button = checkBox.GetComponentInChildren<UIButton>();
                    button.normalBgSprite = "OptionBaseFocused";
                    button.normalFgSprite = spriteName + "Focused";
                }
                else
                {
                    checkBox.isChecked = false;
                    button = checkBox.GetComponentInChildren<UIButton>();
                    button.normalBgSprite = "OptionBase";
                    button.normalFgSprite = spriteName;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:UpdateButtonCheckBox -> Exception: " + e.Message);
            }
        }

        private void UpdateCounters()
        {
            try
            {
                _abandonedCounter.text = Statistics.Instance.AbandonedBuildingsBulldozed.ToString();
                _burnedDownCounter.text = Statistics.Instance.BurnedDownBuildingsBulldozed.ToString();
                _collapsedCounter.text = Statistics.Instance.CollapsedBuildingsBulldozed.ToString();
                _floodedCounter.text = Statistics.Instance.FloodedBuildingsBulldozed.ToString();
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:UpdateCounters -> Exception: " + e.Message);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                _bulldozingStatistics.text = Statistics.Instance.BuildingsBulldozed.ToString();
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:UpdateStatistics -> Exception: " + e.Message);
            }
        }

        private void ResetRunOnce()
        {
            try
            {
                BulldozeManager.Instance.FinishedRunOnce = false;
                BulldozeManager.Instance.IgnoreMaxAtRunOnce = false;

                if (BulldozeManager.Instance.AbandonedBuildingsRunOnce)
                {
                    BulldozeManager.Instance.AbandonedBuildingsRunOnce = false;
                    ModConfig.Instance.AbandonedBuildings = false;
                }
                if (BulldozeManager.Instance.BurnedDownBuildingsRunOnce)
                {
                    BulldozeManager.Instance.BurnedDownBuildingsRunOnce = false;
                    ModConfig.Instance.BurnedDownBuildings = false;
                }
                if (BulldozeManager.Instance.CollapsedBuildingsRunOnce)
                {
                    BulldozeManager.Instance.CollapsedBuildingsRunOnce = false;
                    ModConfig.Instance.CollapsedBuildings = false;
                }
                if (BulldozeManager.Instance.FloodedBuildingsRunOnce)
                {
                    BulldozeManager.Instance.FloodedBuildingsRunOnce = false;
                    ModConfig.Instance.FloodedBuildings = false;
                }

                ModConfig.Instance.Save();
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:ResetRunOnce -> Exception: " + e.Message);
            }
        }
    }
}