using UnityEngine;

namespace Game
{
    public class Spectator : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        private int _index;
        private Camera[] _cameras;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //TODO: redo
                _cameras = FindObjectsOfType<Camera>();
                _index = (_index + 1) % _cameras.Length;
                camera.transform.parent = _cameras[_index].transform;
                camera.transform.localPosition = Vector3.zero;
                camera.transform.localRotation = Quaternion.identity;
            }
        }
    }
}