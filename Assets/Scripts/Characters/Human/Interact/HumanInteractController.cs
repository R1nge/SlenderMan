using Characters.Human.Pickup;
using UnityEngine;

namespace Characters.Human.Interact
{
    public class HumanInteractController : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private Transform camera;
        private Inventory _inventory;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
        }

        private void Update()
        {
            //TODO: show an icon, if object is interactable
            if (Input.GetKeyDown(KeyCode.E))
            {
                Raycast();
            }
        }

        private void Raycast()
        {
            Ray ray = new Ray(camera.position, camera.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance))
            {
                if (hit.transform.TryGetComponent(out IIntractable intractable))
                {
                    intractable.Interact(_inventory);
                }
                else if (hit.transform.TryGetComponent(out IPickupable pickupable))
                {
                    pickupable.Pickup(_inventory);
                }
            }
        }
    }
}