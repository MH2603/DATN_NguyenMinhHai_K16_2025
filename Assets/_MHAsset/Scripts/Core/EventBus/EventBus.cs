using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace  MH.Core
{


    public interface IEventContext
    {
        // This can be left empty as a marker interface
        // Or you can add properties/methods if specific context data is needed
    }

    /// <summary>
    /// Interface for an event bus that allows registering, unregistering, and sending events.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Registers a callback for a specific event type.
        /// </summary>
        /// <typeparam name="T">The event context type.</typeparam>
        /// <param name="callback">The callback to register.</param>
        void Register<T>(UnityAction<T> callback) where T : IEventContext;

        /// <summary>
        /// Unregisters a callback for a specific event type.
        /// </summary>
        /// <typeparam name="T">The event context type.</typeparam>
        /// <param name="callback">The callback to unregister.</param>
        void Unregister<T>(UnityAction<T> callback) where T : IEventContext;

        /// <summary>
        /// Sends an event to all registered listeners.
        /// </summary>
        /// <typeparam name="T">The event context type.</typeparam>
        /// <param name="context">The event context to send.</param>
        void Send<T>(T context) where T : IEventContext;
    }

    public class EventBus : IEventBus
    {
        #region --------- Fields -------

        // Dictionary to store event type to callback mappings
        // int is hashcode of UnityAction
        private readonly Dictionary<Type, Dictionary<int, UnityAction<IEventContext>>> _eventMap = new();

        #endregion

        #region --------- Public Methods -------

        /// <summary>
        /// Registers a callback for a specific event type
        /// </summary>
        /// <typeparam name="T">The event context type</typeparam>
        /// <param name="callback">The callback to register</param>
        public void Register<T>(UnityAction<T> callback) where T : IEventContext
        {
            if (callback == null)
            {
                Debug.LogError("EventBus: Registered callback is null");
            }

            Type eventType = typeof(T);

            if (!_eventMap.TryGetValue(eventType, out var eventHandlers))
            {
                eventHandlers = new Dictionary<int, UnityAction<IEventContext>>();
                _eventMap[eventType] = eventHandlers;
            }

            int callbackHash = callback.GetHashCode();
            eventHandlers[callbackHash] = (context) => callback((T)context);
        }

        /// <summary>
        /// Unregisters a callback for a specific event type
        /// </summary>
        /// <typeparam name="T">The event context type</typeparam>
        /// <param name="callback">The callback to unregister</param>
        public void Unregister<T>(UnityAction<T> callback) where T : IEventContext
        {
            if (callback == null)
            {
                Debug.LogError("EventBus: Unregistered callback is null");
            }

            Type eventType = typeof(T);

            if (_eventMap.TryGetValue(eventType, out var eventHandlers))
            {
                int callbackHash = callback.GetHashCode();
                eventHandlers.Remove(callbackHash);
            }
        }

        /// <summary>
        /// Sends an event to all registered listeners
        /// </summary>
        /// <typeparam name="T">The event context type</typeparam>
        /// <param name="context">The event context to send</param>
        public void Send<T>(T context) where T : IEventContext
        {
            if (context == null)
            {
                Debug.LogError("EvenBus: send a context is null");
            }

            Type eventType = typeof(T);

            if (_eventMap.TryGetValue(eventType, out var eventHandlers))
            {
                foreach (var handler in eventHandlers.Values)
                {
                    handler(context);
                }
            }
        }

        /// <summary>
        /// Clears all registered events and listeners
        /// </summary>
        public void Clear()
        {
            _eventMap.Clear();
        }

        #endregion
    }
}