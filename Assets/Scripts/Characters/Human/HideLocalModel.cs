using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class HideLocalModel : NetworkBehaviour
    {
        [SerializeField] private Renderer renderer;

        private void Start()
        {
            if (!IsOwner) return;
            renderer.enabled = false;
        }
    }
}