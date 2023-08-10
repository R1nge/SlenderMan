﻿using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Game.States
{
    public class GameEntryPoint : NetworkBehaviour
    {
        private PlayerSpawner _playerSpawner;

        private void Awake()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnMapLoaded;
        }

        private void OnMapLoaded(string _, LoadSceneMode __, List<ulong> loaded, List<ulong> ____)
        {
            StateManager.Instance.ChangeState(StateManager.States.Warmup);
        }

        private void Start()
        {
            StateManager.Instance.OnStateChanged += StateChanged;
            if (StateManager.Instance.CurrentState == StateManager.States.Warmup)
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
            NotesManager.Instance.SpawnNotes();
            print("WARMUP");
        }

        private void StartGame()
        {
            print("GAME");
        }

        private void EndGame()
        {
            print("ENDGAME");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StateManager.Instance.OnStateChanged -= StateChanged;
        }
    }
}