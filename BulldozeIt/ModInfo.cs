using ICities;
using System;
using System.Reflection;

namespace BulldozeIt
{
    public class ModInfo : IUserMod
    {
        public string Name => "Bulldoze It!";
        public string Description => "Allows the automatic bulldozing of buildings.";

        private static readonly string[] IntervalLabels =
        {
            "End of Day",
            "End of Month",
            "End of Year",
            "Every 5 seconds",
            "Every 10 seconds",
            "Every 30 seconds"
        };

        private static readonly int[] IntervalValues =
        {
            1,
            2,
            3,
            4,
            5,
            6
        };

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group;

            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();

            group = helper.AddGroup(Name + " - " + assemblyName.Version.Major + "." + assemblyName.Version.Minor);

            bool selected;
            int selectedIndex;
            int selectedValue;
            int result;

            selectedIndex = GetSelectedOptionIndex(IntervalValues, ModConfig.Instance.Interval);
            group.AddDropdown("Interval", IntervalLabels, selectedIndex, sel =>
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

            selected = ModConfig.Instance.IgnoreSearchingForSurvivors;
            group.AddCheckbox("Ignore Searching For Survivors", selected, sel =>
            {
                ModConfig.Instance.IgnoreSearchingForSurvivors = sel;
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
