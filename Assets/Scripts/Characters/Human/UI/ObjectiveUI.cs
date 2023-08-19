using Game.Objectives;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.UI
{
    public class ObjectiveUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI objectivesText;

        private void Awake()
        {
            ObjectiveManager.Instance.OnTaskComplete += UpdateUI;
        }

        private void Start()
        {
            objectivesText.gameObject.SetActive(IsOwner);
            UpdateUI(Task.TaskType.CarBattery);
        }

        private void UpdateUI(Task.TaskType obj)
        {
            var str = "";
            for (int i = 0; i < ObjectiveManager.Instance.Objectives.Count; i++)
            {
                for (int j = 0; j < ObjectiveManager.Instance.Objectives[(ObjectiveManager.ObjectiveType)i].tasks.Count; j++)
                {
                    if (ObjectiveManager.Instance.Objectives[(ObjectiveManager.ObjectiveType)i].tasks[(Task.TaskType)j].completed)
                    {
                        continue;
                    }

                    var objective = ObjectiveManager.Instance.Objectives[(ObjectiveManager.ObjectiveType)i];
                    str += $"Title: {objective.title} Description: {objective.tasks[(Task.TaskType)j].description}; ";
                }
            }

            objectivesText.text = str;
        }
    }
}