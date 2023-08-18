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
        public Dictionary<Task.TaskType, Task> tasks;

        public bool AllTaskCompleted => tasks.All(pair => pair.Value.completed);

        public void CompleteTask(Task.TaskType type)
        {
            tasks[type].completed = true;
        }
    }
}