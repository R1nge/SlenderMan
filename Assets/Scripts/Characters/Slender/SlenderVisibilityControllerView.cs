using System;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Characters.Slender
{
    public class SlenderVisibilityControllerView : NetworkBehaviour
    {
        private Lobby.Lobby _lobby;
        private bool _isVisible;

        public void Construct(Lobby.Lobby lobby)
        {
            _lobby = lobby;
        }

        private void Start()
        {
            if (NetworkObject.IsOwner)
            {
                ChangeVisibilityServerRpc();
            }
        }

        private void Update()
        {
            if (NetworkObject.IsOwner)
            {
                if (Input.GetKeyDown(KeyCode.V))
                {
                    ChangeVisibilityServerRpc();
                }
            }
        }

        [ServerRpc]
        private void ChangeVisibilityServerRpc()
        {
            print("CHANGE");
            _isVisible = !_isVisible;

            print($"PlayerCount: {_lobby.GetPlayerCount()}");
            
            for (int i = 0; i < _lobby.GetPlayerCount(); i++)
            {
                if (NetworkObject.OwnerClientId == _lobby.GetData((ulong)i).Id)
                {
                    continue;       
                }


                if (_isVisible)
                {
                    NetworkObject.NetworkHide(_lobby.GetData((ulong)i).Id);
                }
                else
                {
                    NetworkObject.NetworkShow(_lobby.GetData((ulong)i).Id);
                }
                
                print("CHNAGED");
            }
        }
    }
}