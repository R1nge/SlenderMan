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
        private NetworkVariable<int> _humansAlive, _slendersAlive;
        private bool _loaded;

        private void Awake()
        {
            _humansAlive = new NetworkVariable<int>();
            _slendersAlive = new NetworkVariable<int>();
            _humansAlive.OnValueChanged += (_, newValue) => { print($"Humans alive: {newValue}"); };
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
                    _slendersAlive.Value++;
                    break;
                case Teams.Human:
                    instance = Instantiate(human, position, Quaternion.identity);
                    _humansAlive.Value++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
        }

        //TODO: create a class, that will track player count and change game state
        [ServerRpc(RequireOwnership = false)]
        public void DeSpawnServerRpc(NetworkObjectReference player)
        {
            if (player.TryGet(out NetworkObject networkObject))
            {
                networkObject.Despawn(true);

                switch (Lobby.Lobby.Instance.GetData(networkObject.OwnerClientId).Team)
                {
                    case Teams.Slender:
                        _slendersAlive.Value--;
                        break;
                    case Teams.Human:
                        _humansAlive.Value--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (_humansAlive.Value == 0 || _slendersAlive.Value == 0)
                {
                    StateManager.Instance.ChangeState(StateManager.States.EndGame);
                }
            }
        }
    }
}