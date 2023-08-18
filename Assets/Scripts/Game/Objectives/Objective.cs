using System;
using System.Linq;

namespace Game.Objectives
{
    [Serializable]
    public class Objective
    {
        public string title;
        public Task[] tasks;

        public bool AllTaskCompleted => tasks.All(task => task.completed);

        public void CompleteTask(int index)
        {
            tasks[index].completed = true;
        }
    }
}