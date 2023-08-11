using System;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Slender
{
    public class SlenderVisibilityControllerView : NetworkBehaviour
    {
        private NetworkVariable<bool> _isVisible;

        public NetworkVariable<bool> IsVisible() => _isVisible;

        private void Awake()
        {
            _isVisible = new NetworkVariable<bool>(true);
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
            _isVisible.Value = !_isVisible.Value;
        }
    }
}