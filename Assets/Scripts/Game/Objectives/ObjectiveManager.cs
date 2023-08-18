using System;
using UnityEngine;

namespace Game.Objectives
{
    public class ObjectiveManager : MonoBehaviour
    {
        public event Action<int> OnObjectiveComplete;
        public event Action<int> OnTaskComplete;

        [SerializeField] private Objective[] objectives;
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

        public void CompleteTask(int objectiveIndex, int taskIndex)
        {
            objectives[objectiveIndex].CompleteTask(taskIndex);
            OnTaskComplete?.Invoke(taskIndex);
            Debug.LogError($"Completed task {objectives[objectiveIndex].tasks[taskIndex].description}");
            if (objectives[objectiveIndex].AllTaskCompleted)
            {
                OnObjectiveComplete?.Invoke(objectiveIndex);
                Debug.LogError($"Completed objective {objectives[objectiveIndex].title}");
            }
        }
    }
}