using Sirenix.OdinInspector;
using UnityEngine;

public class RemoveAllColliders : MonoBehaviour
{
    [Button("Remove All Colliders")]
    private void Process(Transform parent, int level = 0)
    {
        foreach (Transform child in parent)
        {
            DestroyImmediate(child.GetComponent<Collider>());
            Process(child, level + 1);
        }
    }
}