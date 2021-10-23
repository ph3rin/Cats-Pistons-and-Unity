using System.Collections;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public abstract class MachineryRenderer<T> : MonoBehaviour, IMachineryRenderer where T : Machinery, ICloneable<T>
    {
        public T CurrentMachinery { get; private set; }
        public MachineryHistory<T> MachineryHistory { get; private set; }
        public IMachineryHistory CreateMachineryHistory(LevelHistory levelHistory)
        {
            Debug.Assert(CurrentMachinery == null);
            CurrentMachinery = CreateMachineryInternal();
            return MachineryHistory = new MachineryHistory<T>(this, levelHistory);
        }
        protected abstract T CreateMachineryInternal();
        public abstract IEnumerator LerpTowards(T dest, float time);

        private Vector2Int RoundPositionToInteger()
        {
            var pos = (Vector2) transform.localPosition;
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        }
    }
}