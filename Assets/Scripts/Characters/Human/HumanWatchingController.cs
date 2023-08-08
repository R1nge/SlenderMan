using Characters.Slender;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class HumanWatchingController : NetworkBehaviour
    {
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
            if (Physics.Raycast(ray, out var hit, distance))
            {
                if (hit.transform.TryGetComponent(out SlenderMovementView _))
                {
                    if (hit.transform.TryGetComponent(out NetworkObject networkObject))
                    {
                        if (networkObject.IsNetworkVisibleTo(NetworkObject.OwnerClientId))
                        {
                            _humanHealthView.ReduceServerRpc(1);
                        }
                    }
                }
            }
        }
    }
}