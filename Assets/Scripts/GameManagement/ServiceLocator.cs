using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatProcessingUnit.GameManagement
{
    public static class ServiceLocator
    {
        private class ServiceState
        {
            public IService Service { get; }
            public bool InitializeCalled { get; set; } = false;

            public ServiceState(IService service)
            {
                Service = service;
            }

            public void Initialize()
            {
                if (InitializeCalled) return;
                InitializeCalled = true;
                Service.Init();
            }
        }

        private static List<ServiceState> _servicesPendingInitialization;
        private static Dictionary<Type, ServiceState> _services;
        private static bool _initializingServices;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeServiceLocator()
        {
            _servicesPendingInitialization = new List<ServiceState>();
            _services = new Dictionary<Type, ServiceState>();
            _initializingServices = false;
        }

        public static void AddService(IService service)
        {
            var state = new ServiceState(service);
            _services.Add(service.GetType(), state);
            _servicesPendingInitialization.Add(state);
        }

        public static void RemoveService(IService service)
        {
            if (_initializingServices)
            {
                throw new Exception($"Cannot remove service while inside method {nameof(InitializeAllServices)}");
            }

            _services.Remove(service.GetType());
        }

        public static T GetService<T>() where T : IService
        {
            if (!_services.TryGetValue(typeof(T), out var serviceState))
            {
                Debug.LogError($"Service of type '{typeof(T)}' is not registered in the service locator");
                return default;
            }

            serviceState.Initialize();
            Debug.Assert(serviceState.Service is T);
            return (T) serviceState.Service;
        }

        public static void InitializeAllServices()
        {
            if (_initializingServices)
            {
                throw new Exception($"Function '{nameof(InitializeAllServices)}' does not allow re-entrance");
            }

            _initializingServices = true;
            var initializedCnt = 0;
            try
            {
                while (_servicesPendingInitialization.Count > 0)
                {
                    initializedCnt += _servicesPendingInitialization.Count;
                    var initList = _servicesPendingInitialization;
                    _servicesPendingInitialization = new List<ServiceState>();
                    foreach (var state in initList)
                    {
                        state.Initialize();
                    }
                }
            }
            finally
            {
                _initializingServices = false;
            }
            Debug.Log($"[INFO] [Frame {Time.frameCount}] Initialized {initializedCnt} services");
        }
    }
}