using System;
using UnityEngine;

namespace CatProcessingUnit.GameManagement
{
    public class RegisterService : MonoBehaviour
    {
        private void Awake()
        {
            foreach (var service in GetComponents<IService>())
            {
                ServiceLocator.AddService(service);
            }
        }
        
        private void OnDestroy()
        {
            foreach (var service in GetComponents<IService>())
            {
                ServiceLocator.RemoveService(service);
            }
        }
    }
}