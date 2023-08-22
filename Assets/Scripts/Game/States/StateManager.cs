using System;
using UnityEngine;

namespace Game.States
{
    public class StateManager : MonoBehaviour
    {
        public static StateManager Instance { get; private set; }
        
        public event Action<States> OnStateChanged; 
        private States _currentState = States.None;

        public States CurrentState => _currentState;
        
        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple StateManagers defined!");
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        
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
            HumanWin,
            SlenderWin
        }
    }
}