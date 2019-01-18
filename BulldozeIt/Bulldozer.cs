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
        private UICheckBox _burnedDownButton;
        private UICheckBox _collapsedButton;
        private UICheckBox _floodedButton;
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
                }
                else
                {
                    _abandonedButton.isVisible = false;
                    _burnedDownButton.isVisible = false;
                    _collapsedButton.isVisible = false;
                    _floodedButton.isVisible = false;
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
                        "FloodedFocused"
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
                _abandonedButton = UIUtils.CreateButtonCheckBox(_tsBar, "BulldozeItAbandoned", _textureAtlas, "Abandoned", "Bulldoze Abandoned Buildings", ModConfig.Instance.AbandonedBuildings);
                _abandonedButton.relativePosition = new Vector3(_bulldozerUndergroundToggle.relativePosition.x - 160f, _bulldozerUndergroundToggle.relativePosition.y);
                _abandonedButton.eventCheckChanged += (component, value) =>
                {
                    ModConfig.Instance.AbandonedBuildings = value;
                    ModConfig.Instance.Save();
                };

                _burnedDownButton = UIUtils.CreateButtonCheckBox(_tsBar, "BulldozeItBurnedDown", _textureAtlas, "BurnedDown", "Bulldoze Burned Down Buildings", ModConfig.Instance.BurnedDownBuildings);
                _burnedDownButton.relativePosition = new Vector3(_bulldozerUndergroundToggle.relativePosition.x - 120f, _bulldozerUndergroundToggle.relativePosition.y);
                _burnedDownButton.eventCheckChanged += (component, value) =>
                {
                    ModConfig.Instance.BurnedDownBuildings = value;
                    ModConfig.Instance.Save();
                };

                _collapsedButton = UIUtils.CreateButtonCheckBox(_tsBar, "BulldozeItCollapsed", _textureAtlas, "Collapsed", "Bulldoze Collapsed Buildings", ModConfig.Instance.CollapsedBuildings);
                _collapsedButton.relativePosition = new Vector3(_bulldozerUndergroundToggle.relativePosition.x - 80f, _bulldozerUndergroundToggle.relativePosition.y);
                _collapsedButton.eventCheckChanged += (component, value) =>
                {
                    ModConfig.Instance.CollapsedBuildings = value;
                    ModConfig.Instance.Save();
                };

                _floodedButton = UIUtils.CreateButtonCheckBox(_tsBar, "BulldozeItFlooded", _textureAtlas, "Flooded", "Bulldoze Flooded Buildings", ModConfig.Instance.FloodedBuildings);
                _floodedButton.relativePosition = new Vector3(_bulldozerUndergroundToggle.relativePosition.x - 40f, _bulldozerUndergroundToggle.relativePosition.y);
                _floodedButton.eventCheckChanged += (component, value) =>
                {
                    ModConfig.Instance.FloodedBuildings = value;
                    ModConfig.Instance.Save();
                };

                _bulldozingStatistics = UIUtils.CreateInfoButton(_happiness, "BulldozeItStatistics", "ToolbarIconBulldozer", "Automatic bulldozed buildings");
                _bulldozingStatistics.size = new Vector2(100f, 26f);
                _bulldozingStatistics.relativePosition = new Vector3(31f, 0f);
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:CreateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateUI()
        {
            try
            {
                UpdateButtonCheckBox(_abandonedButton, "Abandoned", ModConfig.Instance.AbandonedBuildings);
                UpdateButtonCheckBox(_burnedDownButton, "BurnedDown", ModConfig.Instance.BurnedDownBuildings);
                UpdateButtonCheckBox(_collapsedButton, "Collapsed", ModConfig.Instance.CollapsedBuildings);
                UpdateButtonCheckBox(_floodedButton, "Flooded", ModConfig.Instance.FloodedBuildings);

                _bulldozingStatistics.isVisible = ModConfig.Instance.ShowStatistics ? true : false;
            }
            catch (Exception e)
            {
                Debug.Log("[Bulldoze It!] Bulldozer:UpdateUI -> Exception: " + e.Message);
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
    }
}