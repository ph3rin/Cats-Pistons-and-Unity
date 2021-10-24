namespace CatProcessingUnit.Machineries
{
    public interface IMachineryApplication
    {
        Machinery Machinery { get; }
        void ApplyAt(int index);
    }
}