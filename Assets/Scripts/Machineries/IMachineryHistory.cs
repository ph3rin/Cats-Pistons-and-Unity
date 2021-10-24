namespace CatProcessingUnit.Machineries
{
    public interface IMachineryHistory
    {
        IMachineryApplication CopyMachineryAt(int index);
        void MoveForward(int oldIndex, int newIndex);
    }
}