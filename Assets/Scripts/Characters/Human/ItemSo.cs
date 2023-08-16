using UnityEngine;

namespace Characters.Human
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public class ItemSo : ScriptableObject
    {
        [SerializeField] private Sprite icon;

        public Sprite Icon => icon;
    }
}