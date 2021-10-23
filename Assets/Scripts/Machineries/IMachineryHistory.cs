namespace CatProcessingUnit.Machineries
{
    public interface IMachineryHistory
    {
        IMachineryApplication CopyMachineryAt(int index);
    }
}