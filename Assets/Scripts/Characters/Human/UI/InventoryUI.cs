using System;
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
            _inventory.Items.OnListChanged += UpdateUI;
        }

        private void Start()
        {
            inventory.SetActive(IsOwner);
        }

        private void UpdateUI(NetworkListEvent<Item> changeEvent)
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);    
            }
            
            for (int i = 0; i < _inventory.Items.Count; i++)
            {
                var slot = Instantiate(slotPrefab, content);
                slot.SetIcon(Items.ItemData.Instance.GetIcon(_inventory.Items[i].Type));
                slot.SetCount(_inventory.Items[i].Count);
            }
        }

        private void Update()
        {
            if (!IsOwner) return;
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeState();
            }
        }

        private void ChangeState()
        {
            _open = !_open;

            if (!_open)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        private void Open()
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);    
            }
            
            for (int i = 0; i < _inventory.Items.Count; i++)
            {
                var slot = Instantiate(slotPrefab, content);
                slot.SetIcon(Items.ItemData.Instance.GetIcon(_inventory.Items[i].Type));
                slot.SetCount(_inventory.Items[i].Count);
            }
            
            inventory.SetActive(true);
        }

        private void Close()
        {
            inventory.SetActive(false);
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);    
            }
        }
    }
}