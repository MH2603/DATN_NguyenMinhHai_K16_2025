using System;
using System.Collections.Generic;
using UnityEngine;

namespace MH
{
    // Interface representing a general State with three lifecycle methods
    public interface IState
    {
        void OnEnter();  // Called once when entering the state
        void OnUpdate(); // Called every frame (or tick) while in the state
        void OnExit();   // Called once when exiting the state
    }

    // A generic State implementation that uses delegates for behavior
    public abstract class State<T> : IState where T : Enum
    {
        // Delegates that hold the logic for each state phase
        //private readonly Action _onEnter;
        //private readonly Action _onUpdate;
        //private readonly Action _onExit;

        public  StateMachine<T>  _stateMachine;

        // Constructor assigns the passed delegates, falling back to empty methods if null
        public State(StateMachine<T> stateMachine)
        {
            _stateMachine = stateMachine;
            //_onEnter = onEnter ?? (() => { });  // Use no-op if null
            //_onUpdate = onUpdate ?? (() => { });
            //_onExit = onExit ?? (() => { });
        }

        // Executes the associated enter logic
        public abstract void OnEnter();


        // Executes the associated update logic
        public abstract void OnUpdate();


        // Executes the associated exit logic
        public abstract void OnExit();

    }

    // A generic State Machine that manages transitions between states
    public class StateMachine<T> where T : Enum
    {
        // Dictionary mapping enum state keys to their corresponding IState instances
        protected readonly Dictionary<T, IState> _states = new();

        // Currently active state instance
        protected IState _currentState;

        // The enum key of the current state
        protected T _currentKey;

        // Public getter to know what state key is currently active
        public T CurrentKey => _currentKey;

        /// <summary>
        /// Registers a new state with a unique enum key.
        /// If the key is already registered, it will be ignored.
        /// </summary>
        public void RegisterState(T key, IState state)
        {
            if (!_states.ContainsKey(key))
            {
                _states[key] = state;
            }
        }

        /// <summary>
        /// Changes the current state to the specified one by key.
        /// Handles exiting the old state and entering the new one.
        /// Logs a warning if the key is not registered.
        /// </summary>
        public virtual void ChangeState(T key)
        {
            if (_states.TryGetValue(key, out var newState))
            {
                // Exit current state if it exists
                _currentState?.OnExit();

                // Transition to new state
                _currentState = newState;
                _currentKey = key;

                // Enter new state
                _currentState.OnEnter();
            }
            else
            {
                // Developer feedback if state wasn't registered
                Debug.LogWarning($"State {key} not registered.");
            }
        }

        /// <summary>
        /// Calls the update method of the current state.
        /// Typically invoked from MonoBehaviour.Update().
        /// </summary>
        public void Update()
        {
            _currentState?.OnUpdate();
        }
    }


}
