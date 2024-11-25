using System;
using System.Collections;

namespace _Project.Develop.Architecture.Runtime.Utilities.StateMachine
{
    public abstract class FSMState
    {
        protected readonly FSM Fsm;

        public FSMState(FSM fsm)
        {
            Fsm = fsm;
        }

        protected FSMState()
        {
            throw new NotImplementedException();
        }

        public virtual void Enter()
        {
        }

        public virtual IEnumerator Exit()
        {
            yield return null;
        }

        public virtual void Update()
        {
        }
    }
}