namespace PatchPlanner
{
    public class Family
    {
        public int SeedIndex { get; }
        public int Count { get; }

        public Family(int seedIndex, int count)
        {
            this.SeedIndex = seedIndex;
            this.Count = count;
        }
    }
}