using System;
using System.Collections.Generic;
using Game.States;
using Lobby;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class GamePlayersHandler : NetworkBehaviour
    {
        [SerializeField] private Spectator spectator;
        private NetworkVariable<int> _humansAlive, _slendersAlive;
        private PlayerSpawner _playerSpawner;

        private void Awake()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            _playerSpawner.OnPlayerSpawned += Spawned;
            _playerSpawner.OnPlayerDied += Died;
            _humansAlive = new NetworkVariable<int>();
            _slendersAlive = new NetworkVariable<int>();
        }

        private void Spawned(Teams team, GameObject instance, ulong id)
        {
            switch (team)
            {
                case Teams.Slender:
                    _slendersAlive.Value++;
                    break;
                case Teams.Human:
                    _humansAlive.Value++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            print("SPAWNED");
        }

        private void Died(Teams team, GameObject instance, ulong id)
        {
            switch (team)
            {
                case Teams.Slender:
                    _slendersAlive.Value--;
                    break;
                case Teams.Human:
                    _humansAlive.Value--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            if (_humansAlive.Value == 0)
            {
                StateManager.Instance.ChangeState(StateManager.States.SlenderWin);
            }
            else if (_slendersAlive.Value == 0)
            {
                StateManager.Instance.ChangeState(StateManager.States.HumanWin);
            }
            else
            {
                var rpc = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new[]
                        {
                            id
                        }
                    }
                };

                SpawnClientRpc(rpc);
            }

            print("DIED");
        }

        [ClientRpc]
        private void SpawnClientRpc(ClientRpcParams rpcParams)
        {
            Instantiate(spectator);
        }
    }
}