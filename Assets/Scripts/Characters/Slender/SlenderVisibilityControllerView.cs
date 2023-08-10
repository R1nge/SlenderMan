using Unity.Netcode;
using UnityEngine;

namespace Characters.Slender
{
    public class SlenderVisibilityControllerView : NetworkBehaviour
    {
        private bool _isVisible;

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
            _isVisible = !_isVisible;

            var lobby = Lobby.Lobby.Instance;
            
            for (int i = 0; i < lobby.GetPlayerCount(); i++)
            {
                if (NetworkObject.OwnerClientId == lobby.GetData((ulong)i).Id)
                {
                    continue;       
                }
                
                if (_isVisible)
                {
                    NetworkObject.NetworkHide(lobby.GetData((ulong)i).Id);
                }
                else
                {
                    NetworkObject.NetworkShow(lobby.GetData((ulong)i).Id);
                }
            }
        }
    }
}