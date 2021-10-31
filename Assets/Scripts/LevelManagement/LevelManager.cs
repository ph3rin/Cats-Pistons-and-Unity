using System.Collections.Generic;
using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit.LevelManagement
{
    public class LevelManager : MonoBehaviour, IService
    {
        [SerializeField] private List<LevelDescriptor> _levels;

        public IReadOnlyList<LevelDescriptor> Levels => _levels;

        public void Init()
        {
        }

        public int GetLevelId(LevelDescriptor descriptor)
        {
            return _levels.FindIndex(d => ReferenceEquals(d, descriptor));
        }

        public int GetCurrentLevelId()
        {
            var runtimeDescriptor = ServiceLocator.GetService<RuntimeLevelDescriptor>();
            if (runtimeDescriptor == null) return -1;
            return GetLevelId(runtimeDescriptor.StaticDescriptor);
        }

        public LevelDescriptor GetLevel(int id)
        {
            if (id < 0 || id >= _levels.Count) return null;
            return _levels[id];
        }
    }
}