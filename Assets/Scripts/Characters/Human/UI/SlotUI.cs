using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Human.UI
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countText;
        
        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void SetCount(uint count)
        {
            countText.text = count.ToString();
        }
    }
}