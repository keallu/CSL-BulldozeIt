namespace BulldozeIt
{
    public class BulldozeManager
    {
        public bool AbandonedBuildingsRunOnce { get; set; }
        public bool BurnedDownBuildingsRunOnce { get; set; }
        public bool CollapsedBuildingsRunOnce { get; set; }
        public bool FloodedBuildingsRunOnce { get; set; }
        public bool IgnoreMaxAtRunOnce { get; set; }
        public bool FinishedRunOnce { get; set; }

        private static BulldozeManager instance;

        public static BulldozeManager Instance
        {
            get
            {
                return instance ?? (instance = new BulldozeManager());
            }
        }
    }
}