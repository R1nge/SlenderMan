using System;
using Lobby;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class PlayerSpawner : NetworkBehaviour
    {
        public event Action<Teams, GameObject, ulong> OnPlayerSpawned, OnPlayerDied;
        [SerializeField] private GameObject slender, human;
        private bool _loaded;

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
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
            OnPlayerSpawned?.Invoke(team, instance, id);
        }

        [ServerRpc(RequireOwnership = false)]
        public void DeSpawnServerRpc(NetworkObjectReference player, ulong id)
        {
            var team = Lobby.Lobby.Instance.GetData(id).Team;
            if (player.TryGet(out NetworkObject net))
            {
                if (net.OwnerClientId == id && IsServer)
                {
                    OnPlayerDied?.Invoke(team, player, id);
                }
            }
        }
    }
}