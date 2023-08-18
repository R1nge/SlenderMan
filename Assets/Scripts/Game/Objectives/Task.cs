using System;
using UnityEngine;

namespace Game.Objectives
{
    [Serializable]
    public class Task
    {
        [SerializeField] public string description;
        [SerializeField] public bool completed;
    }
}