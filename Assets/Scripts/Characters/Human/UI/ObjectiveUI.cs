using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Human.UI
{
    public class ObjectiveUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI title;

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void SetTitle(string text)
        {
            title.text = text;
        }
    }
}