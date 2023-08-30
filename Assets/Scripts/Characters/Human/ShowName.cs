using System;
using Characters.Human.Interact;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class ShowName : NetworkBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private Transform camera;
        [SerializeField] private TextMeshProUGUI nameText;

        private void Update()
        {
            if (!IsOwner) return;
            var ray = new Ray(camera.position, camera.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance))
            {
                if (hit.transform.TryGetComponent(out InteractIconController human))
                {
                    if (hit.transform.TryGetComponent(out NetworkObject networkObject))
                    {
                        var id = networkObject.OwnerClientId;
                        if (id == OwnerClientId) return;
                        Show(id);
                    }
                    else
                    {
                        Hide();
                    }
                }
                else
                {
                    Hide();
                }
            }
            else
            {
                Hide();
            }
        }

        private void Show(ulong id)
        {
            ShowServerRpc(id);
        }

        private void Hide()
        {
            nameText.enabled = false;
        }

        [ServerRpc(RequireOwnership = false)]
        private void ShowServerRpc(ulong targetId, ServerRpcParams rpcParams = default)
        {
            var name = Lobby.Lobby.Instance.GetData(targetId).Name;

            var rpc = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[]
                    {
                        rpcParams.Receive.SenderClientId
                    }
                }
            };

            ShowClientRpc(name, rpc);
        }

        [ClientRpc]
        private void ShowClientRpc(string name, ClientRpcParams rpcParams)
        {
            nameText.text = name;
            nameText.enabled = true;
        }
    }
}