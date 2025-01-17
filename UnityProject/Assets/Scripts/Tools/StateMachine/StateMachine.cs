using Clovers.Tools;
using UnityEngine;

namespace Clovers.Tools
{
    public class StateMachine<T>
    {
        private IState<T> _state;
        private IState<T> _nextState;

        public void SetNextState(IState<T> nextState)
        {
            _nextState = nextState;
        }

        public void Update(T context)
        {
            if (_nextState != null)
            {
                if(_state != null) Debug.Log($"Exit state: {_state.GetType()}");
                _state?.OnExit(context);
                _state = _nextState;
                _nextState = null;
                
                Debug.Log($"Enter state: {_state.GetType()}");
                _state.OnEnter(context);
            }
            _state?.OnUpdate(context);
        }
    }
}