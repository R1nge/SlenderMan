using Characters.Human;
using Characters.Human.Interact;
using Unity.Netcode;
using UnityEngine;

namespace Cars
{
    public class CarMovement : MonoBehaviour, IInteractable
    {
        public bool Interacted { get; }
        [SerializeField] private Transform[] seats;
        
        //TODO: disable player movement, enable car movement 
        public void Interact(Inventory inventory)
        {
            var player = inventory.GetComponent<NetworkObject>();
            if (player.TrySetParent(seats[0]))
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