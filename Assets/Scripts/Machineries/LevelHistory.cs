using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class LevelHistory : MonoBehaviour, IEnumerable<IMachineryHistory>
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        
        private List<IMachineryHistory> _machineryHistories;
        public int Width => _width;
        public int Height => _height;
        public int ActiveIndex { get; private set; }

        private void Awake()
        {
            _machineryHistories = new List<IMachineryHistory>();
            ActiveIndex = 0;
            foreach (var rdr in transform.GetComponentsInChildren<IMachineryRenderer>())
            {
                _machineryHistories.Add(rdr.CreateMachineryHistory(this));
            }
        }

        public IEnumerator<IMachineryHistory> GetEnumerator()
        {
            return _machineryHistories.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this).GetEnumerator();
        }

        private void MoveForward()
        {
            foreach (var machineryHistory in _machineryHistories)
            {
                machineryHistory.MoveForward(ActiveIndex, ActiveIndex + 1);
            }
            ++ActiveIndex;
        }

        public void Push(List<IMachineryApplication> applications)
        {
            foreach (var app in applications)
            {
                app.ApplyAt(ActiveIndex + 1);
            }

            MoveForward();
        }
    }
}