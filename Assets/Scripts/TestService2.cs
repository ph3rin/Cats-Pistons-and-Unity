using System;
using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit
{
    [RequireComponent(typeof(RegisterService))]
    public class TestService2 : MonoBehaviour, IService
    {
        public void Init()
        {
            Debug.Log("Initialize service 2");
            Debug.Assert(ServiceLocator.GetService<TestService>() != null);
        }

        private void Start()
        {
            Debug.Assert(ServiceLocator.GetService<TestService>() != null);
        }
    }
}