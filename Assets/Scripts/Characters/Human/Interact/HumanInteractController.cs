using Characters.Human.Pickup;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Human.Interact
{
    public class HumanInteractController : NetworkBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private Transform camera;
        [SerializeField] private RawImage hand;
        private Inventory _inventory;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
            hand.enabled = false;
        }

        private void Update()
        {
            if (!IsOwner) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }

            Icon();
        }

        private void Interact()
        {
            Ray ray = new Ray(camera.position, camera.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance))
            {
                if (hit.transform.TryGetComponent(out IIntractable intractable))
                {
                    intractable.Interact(_inventory);
                    return;
                }

                if (hit.transform.TryGetComponent(out IPickupable pickupable))
                {
                    pickupable.Pickup(_inventory);
                }
            }
        }

        private void Icon()
        {
            Ray ray = new Ray(camera.position, camera.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance))
            {
                if (hit.transform.TryGetComponent(out IIntractable _))
                {
                    hand.enabled = true;
                }
                else if (hit.transform.TryGetComponent(out IPickupable _))
                {
                    hand.enabled = true;
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