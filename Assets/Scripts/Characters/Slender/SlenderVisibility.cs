using System;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Slender
{
    public class SlenderVisibility : NetworkBehaviour
    {
        [SerializeField] private MeshRenderer mesh;
        private SlenderVisibilityControllerView _visibilityController;

        private void Awake()
        {
            _visibilityController = GetComponent<SlenderVisibilityControllerView>();
        }

        private void Start()
        {
            if (IsOwner)
            {
                _visibilityController.IsVisible().OnValueChanged += VisibilityChanged;
                ChangeVisibilityServerRpc(false);
            }
        }

        private void VisibilityChanged(bool _, bool isVisible)
        {
            ChangeVisibilityServerRpc(isVisible);
        }

        [ServerRpc]
        private void ChangeVisibilityServerRpc(bool isVisible)
        {
            var lobby = Lobby.Lobby.Instance;

            for (int i = 0; i < lobby.GetPlayerCount(); i++)
            {
                var id = lobby.GetData((ulong)i).Id;

                mesh.enabled = isVisible;

                if (NetworkObject.OwnerClientId != id || id != 0)
                {
                    ChangeVisibilityClientRpc(isVisible);
                }
            }
        }

        [ClientRpc]
        private void ChangeVisibilityClientRpc(bool isVisible)
        {
            mesh.enabled = isVisible;
        }
    }
}