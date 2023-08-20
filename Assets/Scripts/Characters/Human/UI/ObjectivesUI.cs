using Game.Objectives;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.UI
{
    public class ObjectivesUI : NetworkBehaviour
    {
        [SerializeField] private GameObject objectiveCanvas;
        [SerializeField] private Transform objectiveParent;
        [SerializeField] private ObjectiveUI objectivePrefab;
        [SerializeField] private TaskUI taskPrefab;
        private Transform _taskParent;

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
                var objectiveInstance = Instantiate(objectivePrefab, objectiveParent);
                objectiveInstance.SetTitle(objective.title);
                objectiveInstance.SetIcon(objective.icon);
                _taskParent = objectiveInstance.transform.GetChild(2);
                foreach (var task in objective.tasks.Values)
                {
                    var taskInstance = Instantiate(taskPrefab, _taskParent);
                    taskInstance.SetText(task.description);
                }
            }
        }

        private void UpdateUI(ObjectiveManager.ObjectiveType objectiveType, Task.TaskType taskType)
        {
            objectiveParent.GetChild((int)objectiveType).GetChild(2).GetChild((int)taskType).GetComponent<TaskUI>()
                .CompleteText();
        }
    }
}