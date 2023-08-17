using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class Hand : MonoBehaviour
    {
        private Transform _child;

        public void SetChild(Transform child) => _child = child;

        public void DestroyChild()
        {
            Destroy(_child.gameObject);
            _child = null;
        }

        private void Update()
        {
            if (_child != null)
            {
                _child.position = transform.position;
            }
        }
    }
}