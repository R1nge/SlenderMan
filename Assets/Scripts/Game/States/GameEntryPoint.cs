using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using VContainer;

namespace Game.States
{
    public class GameEntryPoint : NetworkBehaviour
    {
        private StateManager _stateManager;
        private PlayerSpawner _playerSpawner;

        [Inject]
        private void Construct(StateManager stateManager, PlayerSpawner playerSpawner)
        {
            _stateManager = stateManager;
            _playerSpawner = playerSpawner;
        }

        private void Awake()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnMapLoaded;
        }

        private void OnMapLoaded(string _, LoadSceneMode __, List<ulong> loaded, List<ulong> ____)
        {
            _stateManager.ChangeState(StateManager.States.Warmup);
        }

        private void Start()
        {
            _stateManager.OnStateChanged += StateChanged;
            if (_stateManager.CurrentState == StateManager.States.Warmup)
            {
                Warmup();
            }
        }

        private void StateChanged(StateManager.States state)
        {
            switch (state)
            {
                case StateManager.States.Warmup:
                    Warmup();
                    break;
                case StateManager.States.Game:
                    StartGame();
                    break;
                case StateManager.States.EndGame:
                    EndGame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void Warmup()
        {
            _playerSpawner.SpawnServerRpc();
            print("WARMUP");
        }

        private void StartGame()
        {
        }

        private void EndGame()
        {
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _stateManager.OnStateChanged -= StateChanged;
        }
    }
}