using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace MH
{
    public static class ServiceLocator
    {
        #region ---------Fields -------

        private static Dictionary<System.Type, object> _serviceMap = new();

        #endregion

        #region --------- Public Methods --------

        static ServiceLocator()
        {

#if UNITY_EDITOR
            // clear static properties when end play mode
            EditorApplication.playModeStateChanged += (data) => Clear();
#endif
        }
            

        /// <summary>
        /// Registers a service instance with the locator
        /// </summary>
        /// <typeparam name="T">The type of service to register</typeparam>
        /// <param name="service">The service instance to register</param>
        public static void Register<T>(T service) where T : class
        {
            if (service == null)
            {
                Debug.LogError($" ServiceLocator: {nameof(service)} is null ");
                return;
            }

            Type type = typeof(T);
            if (_serviceMap.ContainsKey(type))
            {
                Debug.LogError($" ServiceLocator: {nameof(service)} is already registered");
                return;
            }

            _serviceMap[type] = service;
        }

        /// <summary>
        /// Get a service instance from the locator
        /// </summary>
        /// <typeparam name="T">The type of service to resolve</typeparam>
        /// <returns>The registered service instance</returns>
        public static T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (_serviceMap.TryGetValue(type, out object service))
            {
                return (T)service;
            }
            
            throw new KeyNotFoundException($"Service of type {type.Name} not found");
        }

        /// <summary>
        /// Checks if a service is registered
        /// </summary>
        /// <typeparam name="T">The type of service to check</typeparam>
        /// <returns>True if the service is registered, false otherwise</returns>
        public static bool IsRegistered<T>() where T : class
        {
            return _serviceMap.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Unregisters a service from the locator
        /// </summary>
        /// <typeparam name="T">The type of service to unregister</typeparam>
        public static void Unregister<T>() where T : class
        {
            Type type = typeof(T);
            if (!_serviceMap.Remove(type))
            {
                Debug.LogError($"Service of type {type.Name} was not registered");
            }
        }

        /// <summary>
        /// Clears all registered services
        /// </summary>
        public static void Clear()
        {
            _serviceMap.Clear();
        }

        #endregion
    }
}