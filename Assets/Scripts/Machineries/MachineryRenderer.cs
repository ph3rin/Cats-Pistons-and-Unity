using System.Collections;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public abstract class MachineryRenderer<T> : MonoBehaviour, IMachineryRenderer where T : Machinery
    {
        public T Current { get; private set; }

        public IMachineryHistory CreateMachineryHistory(LevelHistory levelHistory)
        {
            Debug.Assert(Current == null);
            Current = CreateMachineryInternal();
            return new MachineryHistory<T>(this, levelHistory);
        }
        protected abstract T CreateMachineryInternal();
        public abstract IEnumerator LerpTowards(T target, float time);
    }
}