using Characters.Human;
using Characters.Human.Interact;
using Unity.Netcode;
using UnityEngine;

namespace Cars
{
    public class CarMovement : MonoBehaviour, IInteractable
    {
        public bool Interacted { get; }
        
        //TODO: destroy player, spawn spectator
        public void Interact(Inventory inventory)
        {
            var player = inventory.GetComponent<NetworkObject>();
            if (player.TrySetParent(transform))
            {
                player.transform.localPosition = Vector3.zero;
                player.transform.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.LogError("Can't change parent", this);
            }
        }
    }
}