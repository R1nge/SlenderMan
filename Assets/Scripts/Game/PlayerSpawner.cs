using System;
using Lobby;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class PlayerSpawner : NetworkBehaviour
    {
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


            //TODO: find a better way
            //I can make a class, which resolves all dependencies and then inject/grab them from it
            instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
        }

        //NetworkManager.Singleton.PrefabHandler
        //NetworkManager.Singleton.PrefabHandler.AddHandler()
    }
}