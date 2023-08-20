using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Objectives
{
    public class ObjectiveManager : SerializedMonoBehaviour
    {
        public enum ObjectiveType
        {
            CarRepair
        }
        public event Action<ObjectiveType> OnObjectiveComplete;
        public event Action<ObjectiveType, Task.TaskType> OnTaskComplete;

        [SerializeField] private Dictionary<ObjectiveType, Objective> objectives;
        public Dictionary<ObjectiveType, Objective> Objectives => objectives;
        public static ObjectiveManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple Objective Managers defined!");
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void CompleteTask(ObjectiveType objectiveType, Task.TaskType taskType)
        {
            objectives[objectiveType].CompleteTask(taskType);
            Debug.LogError($"Completed task {objectives[objectiveType].tasks[taskType].description}");
            OnTaskComplete?.Invoke(objectiveType, taskType);
            if (objectives[objectiveType].AllTaskCompleted)
            {
                Debug.LogError($"Completed objective {objectives[objectiveType].title}");
                OnObjectiveComplete?.Invoke(objectiveType);
            }
        }
    }
}