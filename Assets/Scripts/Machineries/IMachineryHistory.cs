namespace CatProcessingUnit.Machineries
{
    public interface IMachineryHistory
    {
        IMachineryApplication CloneMachineryAt(int index);
        void MoveForward(int oldIndex, int newIndex);
    }
}