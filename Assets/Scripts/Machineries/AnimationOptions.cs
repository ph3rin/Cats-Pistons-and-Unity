namespace CatProcessingUnit.Machineries
{
    public readonly struct AnimationOptions
    {
        public bool Snap { get; }
        public float Time { get; }

        public AnimationOptions(float time)
        {
            Snap = false;
            Time = time;
        }

        private AnimationOptions(bool snap, float time)
        {
            Snap = snap;
            Time = time;
        }

        public static readonly AnimationOptions Instant =  new AnimationOptions(true, 0.0f);
    }
}