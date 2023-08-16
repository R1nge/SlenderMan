using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Data")]
    public class ItemDataSo : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite icon;
        
        public GameObject Prefab => prefab;
        public Sprite Icon => icon;
    }
}