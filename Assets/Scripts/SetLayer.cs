using Characters.Human;
using Game.States;
using Unity.Netcode;
using UnityEngine;

public class SetLayer : NetworkBehaviour
{
    private void Start()
    {
        if (!IsOwner) return;
        StateManager.Instance.OnStateChanged += OnGameStarted;
    }

    private void OnGameStarted(StateManager.States state)
    {
        if (state == StateManager.States.Game)
        {
            var players = FindObjectsByType<HumanMovementView>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < players.Length; i++)
            {
                for (int j = 0; j < players[i].GetComponentsInChildren<Transform>().Length; j++)
                {
                    players[i].GetComponentsInChildren<Transform>()[j].gameObject.layer =
                        LayerMask.NameToLayer("HumanWallHack");
                }
            }
        }
    }
}