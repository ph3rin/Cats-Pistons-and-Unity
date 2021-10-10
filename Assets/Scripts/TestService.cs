using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit
{
    [RequireComponent(typeof(RegisterService))]
    public class TestService : MonoBehaviour, IService
    {
        public void Initialize()
        {
            print("Initialize test service");
            Debug.Assert(ServiceLocator.GetService<TestService2>() != null);
        }
        
        private void Start()
        {
            Debug.Assert(ServiceLocator.GetService<TestService2>() != null);
        }
    }
}