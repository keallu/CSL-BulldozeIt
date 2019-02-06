﻿using ICities;
using System;

namespace BulldozeIt
{
    public class ModInfo : IUserMod
    {
        public string Name => "Bulldoze It!";
        public string Description => "Allows to automate the bulldozing of buildings.";

        private static readonly string[] IntervalLabels =
        {
            "End of Day",
            "End of Month",
            "End of Year"
        };

        private static readonly int[] IntervalValues =
        {
            1,
            2,
            3
        };

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group;
            bool selected;
            int selectedIndex;
            int selectedValue;
            int result;

            group = helper.AddGroup(Name);

            selected = ModConfig.Instance.AbandonedBuildings;
            group.AddCheckbox("Abandoned Buildings", selected, sel =>
            {
                ModConfig.Instance.AbandonedBuildings = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.BurnedDownBuildings;
            group.AddCheckbox("Burned Down Buildings", selected, sel =>
            {
                ModConfig.Instance.BurnedDownBuildings = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.CollapsedBuildings;
            group.AddCheckbox("Collapsed Buildings", selected, sel =>
            {
                ModConfig.Instance.CollapsedBuildings = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.FloodedBuildings;
            group.AddCheckbox("Flooded Buildings", selected, sel =>
            {
                ModConfig.Instance.FloodedBuildings = sel;
                ModConfig.Instance.Save();
            });

            selectedIndex = GetSelectedOptionIndex(IntervalValues, ModConfig.Instance.Interval);
            group.AddDropdown("Interval (in game time)", IntervalLabels, selectedIndex, sel =>
            {
                ModConfig.Instance.Interval = IntervalValues[sel];
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.MaxBuildingsPerInterval;
            group.AddTextfield("Max Buildings (per interval)", selectedValue.ToString(), sel =>
            {
                int.TryParse(sel, out result);
                ModConfig.Instance.MaxBuildingsPerInterval = result;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.PreserveHistoricalBuildings;
            group.AddCheckbox("Preserve Historical Buildings", selected, sel =>
            {
                ModConfig.Instance.PreserveHistoricalBuildings = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.ShowCounters;
            group.AddCheckbox("Show Counters in Bulldozer Bar", selected, sel =>
            {
                ModConfig.Instance.ShowCounters = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.ShowStatistics;
            group.AddCheckbox("Show Statistics in Info Panel", selected, sel =>
            {
                ModConfig.Instance.ShowStatistics = sel;
                ModConfig.Instance.Save();
            });
        }

        private int GetSelectedOptionIndex(int[] option, int value)
        {
            int index = Array.IndexOf(option, value);
            if (index < 0) index = 0;

            return index;
        }
    }
}