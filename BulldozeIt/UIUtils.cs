using ColossalFramework.UI;
using UnityEngine;

namespace BulldozeIt
{
    public class UIUtils
    {
        public static UICheckBox CreateButtonCheckBox(UIComponent parent, string name, UITextureAtlas atlas, string spriteName, string toolTip, bool state)
        {
            UICheckBox checkBox = parent.AddUIComponent<UICheckBox>();
            checkBox.name = name + "CheckBox";
            checkBox.size = new Vector3(36f, 36f);

            UIButton button = checkBox.AddUIComponent<UIButton>();
            button.name = name + "Button";
            button.atlas = atlas;
            button.tooltip = toolTip;
            button.relativePosition = new Vector3(0f, 0f);

            button.normalBgSprite = "OptionBase";
            button.hoveredBgSprite = "OptionBaseHovered";
            button.pressedBgSprite = "OptionBasePressed";
            button.disabledBgSprite = "OptionBaseDisabled";

            button.foregroundSpriteMode = UIForegroundSpriteMode.Fill;
            button.normalFgSprite = spriteName;
            button.hoveredFgSprite = spriteName;
            button.pressedFgSprite = spriteName;
            button.disabledFgSprite = spriteName;

            checkBox.isChecked = state;
            if (state)
            {
                button.normalBgSprite = "OptionBaseFocused";
                button.normalFgSprite = spriteName + "Focused";
            }

            checkBox.eventCheckChanged += (component, value) =>
            {
                if (value)
                {
                    button.normalBgSprite = "OptionBaseFocused";
                    button.normalFgSprite = spriteName + "Focused";
                }
                else
                {
                    button.normalBgSprite = "OptionBase";
                    button.normalFgSprite = spriteName;
                }
            };

            return checkBox;
        }

        public static UIButton CreateInfoButton(UIComponent parent, string name, string spriteName, string toolTip)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = name;
            button.tooltip = toolTip;
            button.textColor = new Color32(185, 221, 254, 255);
            button.hoveredTextColor = new Color32(185, 221, 254, 255);
            button.pressedTextColor = new Color32(185, 221, 254, 255);
            button.focusedTextColor = new Color32(185, 221, 254, 255);
            button.textPadding = new RectOffset(30, 0, 0, 0);
            button.horizontalAlignment = UIHorizontalAlignment.Left;
            button.textHorizontalAlignment = UIHorizontalAlignment.Left;
            button.minimumSize = new Vector2(100f, 26f);
            button.pivot = UIPivotPoint.TopCenter;
            button.anchor = UIAnchorStyle.Left | UIAnchorStyle.Right | UIAnchorStyle.CenterVertical | UIAnchorStyle.Proportional;

            button.normalBgSprite = "InfoDisplay";

            button.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            button.normalFgSprite = spriteName;

            return button;
        }
    }
}
