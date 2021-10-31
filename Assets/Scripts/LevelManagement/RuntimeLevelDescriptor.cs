using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit.LevelManagement
{
    [RequireComponent(typeof(RegisterService))]
    public class RuntimeLevelDescriptor : MonoBehaviour, IService
    {
        [SerializeField] private LevelDescriptor _staticDescriptor;
        public LevelDescriptor StaticDescriptor => _staticDescriptor;

        public void Init()
        {
        }
    }
}