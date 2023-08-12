using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class FlashlightController : NetworkBehaviour
    {
        [SerializeField] private Light light;
        private bool _enabled = true;

        private void Update()
        {
            if (!IsOwner) return;
            if (Input.GetKeyDown(KeyCode.F))
            {
                SwitchServerRpc();
            }
        }

        [ServerRpc]
        private void SwitchServerRpc()
        {
            _enabled = !_enabled;
            light.enabled = _enabled;
            SwitchClientRpc(_enabled);
        }

        [ClientRpc]
        private void SwitchClientRpc(bool enabled)
        {
            if (IsServer) return;
            light.enabled = enabled;
        }
    }
}