using CatProcessingUnit.Machineries;

namespace CatProcessingUnit.Tests
{
    public static class MachineryTestingExt
    {
        private class NoOpMachineryApplication : IMachineryApplication
        {
            public NoOpMachineryApplication(Machinery machinery)
            {
                Machinery = machinery;
            }

            public Machinery Machinery { get; }
            public void Apply()
            {
            }
        }

        public static IMachineryApplication NoOpApplication(this Machinery self)
        {
            return new NoOpMachineryApplication(self);
        }
    }
}