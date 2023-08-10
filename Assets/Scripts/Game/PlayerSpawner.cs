using System;
using Game.States;
using Lobby;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Game
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject slender, human;
        private NetworkVariable<int> _playersAlive;
        private bool _loaded;

        private void Awake()
        {
            _playersAlive = new NetworkVariable<int>();
            _playersAlive.OnValueChanged += (value, newValue) => { print(newValue); };
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnServerRpc(ServerRpcParams rpcParams = default)
        {
            GameObject instance;
            var id = rpcParams.Receive.SenderClientId;
            var team = Lobby.Lobby.Instance.GetData(id).Team;
            var position = new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));

            switch (team)
            {
                case Teams.Slender:
                    instance = Instantiate(slender, position, Quaternion.identity);
                    break;
                case Teams.Human:
                    instance = Instantiate(human, position, Quaternion.identity);
                    _playersAlive.Value++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
        }

        //TODO: create a class, that will track player count and change game state
        [ServerRpc(RequireOwnership = false)]
        public void DeSpawnServerRpc(ulong id)
        {
            var player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[id];
            player.Despawn(true);
            _playersAlive.Value--;

            if (_playersAlive.Value == 0)
            {
                StateManager.Instance.ChangeState(StateManager.States.EndGame);
            }
        }
    }
}