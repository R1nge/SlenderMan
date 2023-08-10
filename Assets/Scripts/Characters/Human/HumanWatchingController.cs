using Characters.Slender;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class HumanWatchingController : NetworkBehaviour
    {
        [SerializeField] private LayerMask ignore;
        [SerializeField] private float distance;
        [SerializeField] private Transform playerCamera;
        private HumanHealthView _humanHealthView;

        private void Awake()
        {
            _humanHealthView = GetComponent<HumanHealthView>();
        }

        private void Update()
        {
            CheckForSlender();
        }

        private void CheckForSlender()
        {
            var ray = new Ray(playerCamera.position, playerCamera.forward);
            if (!Physics.Raycast(ray, out var hit, distance, ~ignore)) return;
            if (!hit.transform.TryGetComponent(out NetworkObject networkObject)) return;
            if (!networkObject.IsNetworkVisibleTo(NetworkObject.OwnerClientId)) return;
            if (!networkObject.TryGetComponent(out SlenderVisibilityControllerView visibility)) return;
            if (!visibility.IsVisible()) return;
            _humanHealthView.ReduceServerRpc(1);
        }
    }
}