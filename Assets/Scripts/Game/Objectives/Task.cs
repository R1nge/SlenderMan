using System;
using UnityEngine;

namespace Game.Objectives
{
    [Serializable]
    public class Task
    {
        public enum TaskType
        {
            CarFuel
        }

        [SerializeField] public string description;
        [SerializeField] public bool completed;
    }
}