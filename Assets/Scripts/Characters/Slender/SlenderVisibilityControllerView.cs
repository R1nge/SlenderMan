using Unity.Netcode;
using UnityEngine;

namespace Characters.Slender
{
    public class SlenderVisibilityControllerView : NetworkBehaviour
    {
        [SerializeField] private MeshRenderer mesh;
        private bool _isVisible;

        public bool IsVisible() => _isVisible;

        private void Start()
        {
            _isVisible = mesh.enabled || NetworkObject.IsNetworkVisibleTo(NetworkObject.OwnerClientId);
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

                if (lobby.GetData((ulong)i).Id == 0)
                {
                    mesh.enabled = _isVisible;
                    return;
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