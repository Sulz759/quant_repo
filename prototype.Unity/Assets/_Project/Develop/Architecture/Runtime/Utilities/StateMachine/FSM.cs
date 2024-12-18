﻿using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Architecture.Runtime.Utilities.Logging;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Utilities.StateMachine
{
    public class FSM
    {
        private readonly Dictionary<Type, FSMState> _states = new();

        private Coroutine _transition;
        public FSMState StateCurrent { get; private set; }

        public bool isTransitioning { get; private set; }

        public void AddState(FSMState state)
        {
            _states.Add(state.GetType(), state);
        }

        public void SetState<T>() where T : FSMState
        {
            var type = typeof(T);

            if (StateCurrent != null && StateCurrent.GetType() == type) return;

            if (_states.TryGetValue(type, out var newState)) Coroutines.StartRoutine(TransitionState(newState));
        }

        private IEnumerator TransitionState(FSMState state)
        {
            if (isTransitioning)
            {
                yield break;
            }
            
            isTransitioning = true;
            yield return StateCurrent switch
            {
                null => null,
                _ => Coroutines.StartRoutine(StateCurrent?.Exit())
            };

            StateCurrent = state;
            
            StateCurrent.Enter();

            isTransitioning = false;
        }

        public void Update()
        {
            StateCurrent?.Update();
        }
    }
}