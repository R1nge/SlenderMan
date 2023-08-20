using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Objectives
{
    [CreateAssetMenu(menuName = "Objective", fileName = "Objective")]
    public class Objective : SerializedScriptableObject
    {
        public string title;
        public Sprite icon;
        public Dictionary<Task.TaskType, Task> tasks;

        public bool AllTaskCompleted => tasks.All(pair => pair.Value.completed);

        private void OnEnable()
        {
            foreach (var task in tasks)
            {
                task.Value.completed = false;
            }
        }

        private void OnValidate()
        {
            foreach (var task in tasks)
            {
                task.Value.completed = false;
            }
        }

        public void CompleteTask(Task.TaskType type)
        {
            tasks[type].completed = true;
        }
    }
}