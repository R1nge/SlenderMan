using UnityEngine;
using UnityEngine.UI;

namespace Characters.Human.UI
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }
    }
}