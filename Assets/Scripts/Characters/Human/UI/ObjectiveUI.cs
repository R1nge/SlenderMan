using System;
using System.Collections.Generic;
using Game.Objectives;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.UI
{
    public class ObjectiveUI : NetworkBehaviour
    {
        [SerializeField] private GameObject objectiveCanvas;
        [SerializeField] private Transform objectiveParent, taskParent;
        [SerializeField] private TaskUI taskPrefab;

        private void Awake()
        {
            ObjectiveManager.Instance.OnTaskComplete += UpdateUI;
        }

        private void Start()
        {
            objectiveCanvas.gameObject.SetActive(false);
            InitUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (objectiveCanvas.activeInHierarchy)
                {
                    objectiveCanvas.SetActive(false);
                }
                else
                {
                    objectiveCanvas.SetActive(true);
                }
            }
        }

        private void InitUI()
        {
            foreach (var objective in ObjectiveManager.Instance.Objectives.Values)
            {
                foreach (var task in objective.tasks.Values)
                {
                    var taskInstance = Instantiate(taskPrefab, taskParent);
                    taskInstance.SetText(task.description);
                }
            }
        }

        private void UpdateUI(ObjectiveManager.ObjectiveType objectiveType, Task.TaskType taskType)
        {
            objectiveParent.GetChild((int)objectiveType).GetChild(2).GetChild((int)taskType).GetComponent<TaskUI>().CompleteText();
        }
    }
}