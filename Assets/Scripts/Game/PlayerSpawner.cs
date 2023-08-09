using System;
using System.Collections.Generic;
using Characters.Slender;
using Game.States;
using Lobby;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Game
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject slender, human;
        private IObjectResolver _objectResolver;
        private Lobby.Lobby _lobby;
        private StateManager _stateManager;

        [Inject]
        private void Construct(IObjectResolver objectResolver, Lobby.Lobby lobby, StateManager stateManager)
        {
            _objectResolver = objectResolver;
            _lobby = lobby;
            _stateManager = stateManager;
        }

        private void Awake()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoad;
        }

        private void OnLoad(string _, LoadSceneMode __, List<ulong> loaded, List<ulong> ____)
        {
            SpawnServerRpc(loaded.Count);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnServerRpc(int loadedCount, ServerRpcParams rpcParams = default)
        {
            if (loadedCount != _lobby.GetPlayerCount()) return;
            if (_stateManager.CurrentState != StateManager.States.Warmup) return;
            
            GameObject instance;
            var id = rpcParams.Receive.SenderClientId;
            var team = _lobby.GetData(id).Team;
            var position = new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));
            switch (team)
            {
                case Teams.Slender:
                    instance = _objectResolver.Instantiate(slender, position, Quaternion.identity);
                    break;
                case Teams.Human:
                    instance = _objectResolver.Instantiate(human, position, Quaternion.identity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);

            //TODO: find a better way
            //I can make a class, which resolves all dependencies and then inject/grab them from it
            if (instance.TryGetComponent(out SlenderVisibilityControllerView slenderVisibility))
            {
                slenderVisibility.Construct(_lobby);
            }
        }
    }
}