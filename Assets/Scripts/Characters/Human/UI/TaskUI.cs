using TMPro;
using UnityEngine;

namespace Characters.Human.UI
{
    public class TaskUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetText(string newText)
        {
            text.text = newText;
        }

        public void CompleteText()
        {
            text.text = $"<s>{text.text}</s>";
        }
    }
}