using System.Collections;
using Game.States;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Game
{
    public class TimerView : NetworkBehaviour
    {
        [SerializeField] private int timeBeforeStart;
        private NetworkVariable<int> _currentTime;
        private StateManager _stateManager;

        public NetworkVariable<int> CurrentTime => _currentTime;

        [Inject]
        private void Construct(StateManager stateManager)
        {
            _stateManager = stateManager;
        }

        private void Awake()
        {
            _currentTime = new NetworkVariable<int>(timeBeforeStart);
        }

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                StartCoroutine(Timer());
            }
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(1);

            _currentTime.Value -= 1;

            if (_currentTime.Value == 0)
            {
                _stateManager.ChangeState(StateManager.States.Game);
                yield break;
            }

            StartCoroutine(Timer());
        }
    }
}