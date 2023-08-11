using System;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class HealthColor : NetworkBehaviour
    {
        [SerializeField] private Renderer mesh;
        [SerializeField] private Gradient gradient;
        private HumanHealthView _humanHealth;
        private ulong _slender;

        private void Awake()
        {
            _humanHealth = GetComponent<HumanHealthView>();
            _humanHealth.CurrentHealth.OnValueChanged += OnValueChanged;
        }

        private void Start()
        {
            GetSlenderIdServerRpc();

            var rpc = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[]
                    {
                        _slender
                    }
                }
            };

            var val = (float)_humanHealth.CurrentHealth.Value / _humanHealth.MaxHealth.Value;

            UpdateColorClientRpc(val, rpc);
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetSlenderIdServerRpc()
        {
            var slender = Lobby.Lobby.Instance.GetSlenderId();
            _slender = slender;
            GetSlenderIdClientRpc(slender);
        }

        [ClientRpc]
        private void GetSlenderIdClientRpc(ulong slender)
        {
            if (IsServer) return;
            _slender = slender;
        }

        private void OnValueChanged(int _, int value)
        {
            var rpc = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[]
                    {
                        _slender
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