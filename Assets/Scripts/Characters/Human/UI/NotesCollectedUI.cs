using Game;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.UI
{
    public class NotesCollectedUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI notesText;
        private NotesManager _notesManager;

        private void Awake()
        {
            _notesManager = FindObjectOfType<NotesManager>();
        }

        private void Start()
        {
            notesText.gameObject.SetActive(IsOwner);
            if (!IsOwner) return;
            _notesManager.CollectedAmount().OnValueChanged += NotesChanged;
            NotesChanged(0, _notesManager.CollectedAmount().Value);
        }

        private void NotesChanged(int _, int notesCollected)
        {
            UpdateUI(notesCollected);
        }

        private void UpdateUI(int notesCollected)
        {
            notesText.text = $"Notes: {notesCollected} / {_notesManager.TotalAmount()}";
        }
    }
}