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

        public (Workspace, T) CreateWorkspaceFromActiveIndex()
        {
            var activeIndex = LevelHistory.ActiveIndex;
            MachineryApplication self = null;
            var pendingApplicationMachineries = new List<IMachineryApplication>();
            foreach (var machineryHistory in LevelHistory)
            {
                IMachineryApplication machinery;
                if (ReferenceEquals(machineryHistory, this))
                {
                    machinery = self = CopyMachineryAtInternal(activeIndex);
                }
                else
                {
                    machinery = machineryHistory.CopyMachineryAt(activeIndex);
                }

                pendingApplicationMachineries.Add(machinery);
            }

            Debug.Assert(self != null);
            return (
                new Workspace(pendingApplicationMachineries, LevelHistory.Width, LevelHistory.Height),
                self.MachineryInternal);
        }

        private class MachineryApplication : IMachineryApplication
        {
            private readonly MachineryHistory<T> _machineryHistory;
            public T MachineryInternal { get; }
            public int Index { get; }
            public Machinery Machinery => MachineryInternal;

            public MachineryApplication(MachineryHistory<T> machineryHistory, T machinery, int index)
            {
                _machineryHistory = machineryHistory;
                MachineryInternal = machinery;
                Index = index;
            }


            public void Apply()
            {
                _machineryHistory.ApplyAt(Index, MachineryInternal);
            }
        }

        private void ApplyAt(int index, T machinery)
        {
            Debug.Assert(index >= 1 && index <= _history.Count);
            if (index == _history.Count)
            {
                _history.Add(machinery);
            }
            else
            {
                _history[index] = machinery;
                _history.RemoveRange(index + 1, _history.Count - index - 1);
            }
        }

        public IMachineryApplication CopyMachineryAt(int index)
        {
            return CopyMachineryAtInternal(index);
        }

        public void MoveForward(int oldIndex, int newIndex)
        {
            LevelHistory.StartCoroutine(_renderer.LerpTowards(_history[newIndex], 0.125f));
        }

        private MachineryApplication CopyMachineryAtInternal(int index)
        {
            return new MachineryApplication(this, _history[index].Clone(), index + 1);
        }
    }
}