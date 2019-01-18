namespace BulldozeIt
{
    public class Statistics
    {
        public int AbandonedBuildingsBulldozed { get; set; }
        public int BurnedDownBuildingsBulldozed { get; set; }
        public int CollapsedBuildingsBulldozed { get; set; }
        public int FloodedBuildingsBulldozed { get; set; }

        public int BuildingsBulldozed => AbandonedBuildingsBulldozed + BurnedDownBuildingsBulldozed + CollapsedBuildingsBulldozed + FloodedBuildingsBulldozed;

        private static Statistics instance;

        public static Statistics Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Statistics();
                }

                return instance;
            }
        }
    }
}
