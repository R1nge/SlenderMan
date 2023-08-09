using TMPro;
using UnityEngine;

namespace Game
{
    public class TimerUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        private TimerView _timerView;

        private void Awake()
        {
            _timerView = GetComponent<TimerView>();
        }

        private void Start()
        {
            timerText.text = _timerView.CurrentTime.Value.ToString();
            _timerView.CurrentTime.OnValueChanged += UpdateUI;
        }

        private void UpdateUI(int _, int time)
        {
            timerText.text = time.ToString();
        }
    }
}