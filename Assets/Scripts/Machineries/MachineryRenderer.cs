using System.Collections;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public abstract class MachineryRenderer<T> : MonoBehaviour, IMachineryRenderer where T : Machinery, ICloneable<T>
    {
        public T CurrentMachinery { get; protected set; }
        public MachineryHistory<T> MachineryHistory { get; private set; }
        public IMachineryHistory CreateMachineryHistory(LevelHistory levelHistory)
        {
            Debug.Assert(CurrentMachinery == null);
            CurrentMachinery = CreateMachineryInternal();
            return MachineryHistory = new MachineryHistory<T>(this, levelHistory);
        }
        protected abstract T CreateMachineryInternal();
        public abstract IEnumerator LerpTowards(T dest, float time);
    }
}