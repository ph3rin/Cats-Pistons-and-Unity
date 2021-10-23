using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit.Machineries
{
    public class LevelHistory : MonoBehaviour
    {
        private List<IMachineryHistory> _machineryHistories;

        private void Awake()
        {
            _machineryHistories = new List<IMachineryHistory>();
            foreach (var rdr in transform.GetComponentsInChildren<IMachineryRenderer>())
            {
                _machineryHistories.Add(rdr.CreateMachineryHistory(this));
            }
        }
    }
}