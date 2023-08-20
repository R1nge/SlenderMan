using UnityEngine;
using UnityEngine.UI;

namespace Characters.Human.UI
{
    public class TasksExpand : MonoBehaviour
    {
        [SerializeField] private LayoutElement objective, tasks;

        public void Expand()
        {
            objective.minHeight += tasks.minHeight;
        }
    }
}