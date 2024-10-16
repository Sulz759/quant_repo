using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Develop.Architecture.Runtime.Utilities.StateMachine
{
	public class FSM
	{
		public FSMState StateCurrent {get;private set;}

		private Dictionary<Type, FSMState> _states = new Dictionary<Type, FSMState>();

		public bool isTransitioning { get; private set; }

		private Coroutine _transition;

		public void AddState(FSMState state) 
		{
			_states.Add(state.GetType(), state);
		}

		public void SetState<T>() where T : FSMState
		{
			var type = typeof(T);

			if (StateCurrent != null && StateCurrent.GetType() == type) 
			{
				return;
			}

			if (_states.TryGetValue(type, out var newState))
			{
				Coroutines.StartRoutine(TransitionState(newState));
			}
		}
		
		private IEnumerator TransitionState(FSMState state)
		{
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