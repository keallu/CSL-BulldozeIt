namespace BulldozeIt
{
    [ConfigurationPath("BulldozeItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; }
        public bool AbandonedBuildings { get; set; }
        public bool BurnedDownBuildings { get; set; }
        public bool CollapsedBuildings { get; set; }
        public bool FloodedBuildings { get; set; }
        public int Interval { get; set; } = 1;
        public int MaxBuildingsPerInterval { get; set; } = 32;
        public bool PreserveHistoricalBuildings { get; set; } = true;
        public bool ShowCounters { get; set; } = true;
        public bool ShowStatistics { get; set; } = true;

        private static ModConfig instance;

        public static ModConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Configuration<ModConfig>.Load();
                }

                return instance;
            }
        }

        public void Save()
        {
            Configuration<ModConfig>.Save();
            ConfigUpdated = true;
        }
    }
}