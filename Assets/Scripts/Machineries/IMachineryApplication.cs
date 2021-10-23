namespace CatProcessingUnit.Machineries
{
    public interface IMachineryApplication
    {
        Machinery Machinery { get; }
        void Apply();
    }
}