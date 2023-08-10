using System;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class HealthColor : NetworkBehaviour
    {
        [SerializeField] private MeshRenderer mesh;
        [SerializeField] private Gradient gradient;
        private HumanHealthView _humanHealth;

        private void Awake()
        {
            _humanHealth = GetComponent<HumanHealthView>();
            _humanHealth.CurrentHealth.OnValueChanged += OnValueChanged;
        }

        private void Start()
        {
            GetSlenderIdServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetSlenderIdServerRpc()
        {
            var slender = Lobby.Lobby.Instance.GetSlenderId();

            var rpc = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[]
                    {
                        slender
                    }
                }
            };

            SetSlenderIdClientRpc(rpc);
        }

        [ClientRpc]
        private void SetSlenderIdClientRpc(ClientRpcParams rpcParams)
        {
            mesh.material.color = gradient.Evaluate(1);
        }

        private void OnValueChanged(int _, int value)
        {
            var slender = Lobby.Lobby.Instance.GetSlenderId();

            var rpc = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[]
                    {
                        slender
                    }
                }
            };

            var val = (float)value / _humanHealth.MaxHealth.Value;

            UpdateColorClientRpc(val, rpc);
        }

        [ClientRpc]
        private void UpdateColorClientRpc(float val, ClientRpcParams rpcParams)
        {
            mesh.material.color = gradient.Evaluate(val);
            print(val);
        }
    }
}