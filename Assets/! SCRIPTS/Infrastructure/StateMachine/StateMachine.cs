using System;
using System.Collections.Generic;

namespace Utility.StateMachine
{
    public class StateMachine
    {
        #region FIELDS PRIVATE
        private readonly Dictionary<Type, IState> _states = new();
        private IState _currentState;
        #endregion

        #region METHODS PUBLIC
        public void Initialization(List<IState> states)
        {
            foreach (var state in states)
            {
                _states.Add(state.GetType(), state);
            }
        }

        public void ChangeState<T>() where T : IState
        {
            _currentState?.Exit();
            _currentState = _states[typeof(T)];
            _currentState.Enter();
        }
        #endregion
    }
}
