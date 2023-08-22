using System;

namespace NoClearGames.Patterns.State
{
    public abstract class StateManagerBase<T> where T : IState
    {
        private T _currentState;
        private T _previousState;

        public Action<T, T> OnStateChanged;

        public T CurrentState => _currentState;


        public virtual void ChangeState(T state)
        {
            _currentState?.Dispose();
            _previousState = _currentState;
            _currentState = state;
            _currentState.Initialize();
            OnStateChanged?.Invoke(_previousState, _currentState);
        }

        public bool IsOnState(Type state)
        {
            return state == _currentState.GetType();
        }
    }
}
