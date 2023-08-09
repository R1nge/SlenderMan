using System;

namespace Game.States
{
    public class StateManager
    {
        public event Action<States> OnStateChanged; 
        private States _currentState = States.None;

        public States CurrentState => _currentState;
        
        public void ChangeState(States state)
        {
            _currentState = state;
            OnStateChanged?.Invoke(_currentState);
        }

        public enum States
        {
            None,
            Warmup,
            Game,
            EndGame
        }
    }
}