using Characters.Human.Pickup;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Human.Interact
{
    public class InteractIconController : NetworkBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private Transform camera;
        [SerializeField] private RawImage hand;

        private void Awake() => hand.enabled = false;

        private void Update()
        {
            if (!IsOwner) return;
            Icon();
        }

        private void Icon()
        {
            Ray ray = new Ray(camera.position, camera.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance))
            {
                if (hit.transform.TryGetComponent(out IInteractable intractable))
                {
                    hand.enabled = !intractable.Interacted;
                }
                else if (hit.transform.TryGetComponent(out Pickupable pickupable))
                {
                    hand.enabled = !pickupable.HasOwner;
                }
                else
                {
                    hand.enabled = false;
                }
            }
            else
            {
                hand.enabled = false;
            }
        }
    }
}