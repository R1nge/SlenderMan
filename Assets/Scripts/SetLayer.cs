using Characters.Human;
using Game.States;
using Unity.Netcode;
using UnityEngine;

public class SetLayer : NetworkBehaviour
{
    private void Awake()
    {
        StateManager.Instance.OnStateChanged += OnGameStarted;
    }

    private void OnGameStarted(StateManager.States state)
    {
        if (state == StateManager.States.Game)
        {
            var players = FindObjectsByType<HumanMovementView>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var player in players)
            {
                player.gameObject.layer = LayerMask.NameToLayer("Human");
            }
        }
    }
}