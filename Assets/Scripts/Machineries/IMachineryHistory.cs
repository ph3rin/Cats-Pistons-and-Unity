using System.Collections;

namespace CatProcessingUnit.Machineries
{
    public interface IMachineryHistory
    {
        IMachineryApplication CloneMachineryAt(int index);
        IEnumerator MoveForward(int oldIndex, int newIndex, AnimationOptions animationOptions = default);
        void SetIndex(int index, AnimationOptions animationOptions);
    }
}