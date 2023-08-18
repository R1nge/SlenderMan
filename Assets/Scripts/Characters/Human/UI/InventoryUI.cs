using Items;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.UI
{
    public class InventoryUI : NetworkBehaviour
    {
        [SerializeField] private GameObject inventory;
        [SerializeField] private Transform content;
        [SerializeField] private SlotUI slotPrefab;
        private Inventory _inventory;
        private bool _open;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
            _inventory.PocketItems.OnListChanged += UpdateUI;
        }

        private void Start() => inventory.SetActive(IsOwner);

        private void UpdateUI(NetworkListEvent<Item> changeEvent)
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);    
            }
            
            for (int i = 0; i < _inventory.PocketItems.Count; i++)
            {
                var slot = Instantiate(slotPrefab, content);
                slot.SetIcon(ItemDataManager.Instance.GetIcon(_inventory.PocketItems[i].itemType));
                slot.SetCount(_inventory.PocketItems[i].count);
            }
        }
    }
}