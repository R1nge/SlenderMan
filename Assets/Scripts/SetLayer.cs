using System.Collections;
using Characters.Human;
using Game.States;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class SetLayer : NetworkBehaviour
{
    private StateManager _stateManager;

    [Inject]
    private void Construct(StateManager stateManager)
    {
        _stateManager = stateManager;
    }

    private void Start()
    {
        _stateManager.OnStateChanged += OnGameStarted;
    }

    private void OnGameStarted(StateManager.States state)
    {
        if (!NetworkObject.IsOwner) return;
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