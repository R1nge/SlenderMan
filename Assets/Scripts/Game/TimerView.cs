using System.Collections;
using Game.States;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class TimerView : NetworkBehaviour
    {
        [SerializeField] private int timeBeforeStart;
        private NetworkVariable<int> _currentTime;

        public NetworkVariable<int> CurrentTime => _currentTime;

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
                ChangeState();
                yield break;
            }

            StartCoroutine(Timer());
        }

        private void ChangeState()
        {
            StateManager.Instance.ChangeState(StateManager.States.Game);
            ChangeStateClientRpc();
            print("Changed state SERVER");
        }

        [ClientRpc]
        private void ChangeStateClientRpc()
        {
            if(IsServer) return;
            StateManager.Instance.ChangeState(StateManager.States.Game);
            print("Changed state CLIENT");
        }
    }
}