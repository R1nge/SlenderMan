using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Objectives
{
    public class ObjectiveManager : SerializedMonoBehaviour
    {
        public event Action<int> OnObjectiveComplete;
        public event Action<Task.TaskType> OnTaskComplete;

        [SerializeField] private Objective[] objectives;
        public Objective[] Objectives => objectives;
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

        public void CompleteTask(int objectiveIndex, Task.TaskType type)
        {
            objectives[objectiveIndex].CompleteTask(type);
            OnTaskComplete?.Invoke(type);
            Debug.LogError($"Completed task {objectives[objectiveIndex].tasks[type].description}");
            if (objectives[objectiveIndex].AllTaskCompleted)
            {
                OnObjectiveComplete?.Invoke(objectiveIndex);
                Debug.LogError($"Completed objective {objectives[objectiveIndex].title}");
            }
        }
    }
}