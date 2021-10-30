using System.Collections;
using System.Collections.Generic;
using log4net.Core;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class MachineryHistory<T> : IMachineryHistory where T : Machinery, ICloneable<T>
    {
        private readonly List<T> _history = new List<T>();
        private readonly MachineryRenderer<T> _renderer;

        public LevelHistory LevelHistory { get; }

        public MachineryHistory(MachineryRenderer<T> renderer, LevelHistory levelHistory)
        {
            _renderer = renderer;
            _history.Add(renderer.CurrentMachinery);
            LevelHistory = levelHistory;
        }

        public T this[int i] => _history[i];
        
        public (List<IMachineryApplication>, T) SliceAt(int index)
        {
            MachineryApplication self = null;
            var pendingApplicationMachineries = new List<IMachineryApplication>();
            foreach (var machineryHistory in LevelHistory)
            {
                IMachineryApplication machinery;
                if (ReferenceEquals(machineryHistory, this))
                {
                    machinery = self = CloneMachineryAtInternal(index);
                }
                else
                {
                    machinery = machineryHistory.CloneMachineryAt(index);
                }

                pendingApplicationMachineries.Add(machinery);
            }

            Debug.Assert(self != null);
            return (
                pendingApplicationMachineries,
                self.MachineryInternal);
        }

        private class MachineryApplication : IMachineryApplication
        {
            private readonly MachineryHistory<T> _machineryHistory;
            public T MachineryInternal { get; }
            public Machinery Machinery => MachineryInternal;

            public MachineryApplication(MachineryHistory<T> machineryHistory, T machinery)
            {
                _machineryHistory = machineryHistory;
                MachineryInternal = machinery;
            }


            public void ApplyAt(int index)
            {
                _machineryHistory.SetMachinery(index, MachineryInternal);
            }
        }

        private void SetMachinery(int index, T machinery)
        {
            Debug.Assert(index >= 1 && index <= _history.Count);
            if (index == _history.Count)
            {
                _history.Add(machinery);
            }
            else
            {
                _history[index] = machinery;
            }
        }

        public IMachineryApplication CloneMachineryAt(int index)
        {
            return CloneMachineryAtInternal(index);
        }

        public IEnumerator MoveForward(int oldIndex, int newIndex, AnimationOptions animationOptions)
        {
            var time = animationOptions.Snap ? 0.01f : animationOptions.Time;
            return _renderer.LerpTowards(_history[newIndex], time);
        }

        public void SetIndex(int index, AnimationOptions animationOptions)
        {
            var time = animationOptions.Snap ? 0.01f : animationOptions.Time;
            LevelHistory.StartCoroutine(_renderer.LerpTowards(_history[index], time));
        }

        private MachineryApplication CloneMachineryAtInternal(int index)
        {
            return new MachineryApplication(this, _history[index].Clone());
        }
    }
}