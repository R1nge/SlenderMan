using System;
using Game.Objectives;
using Items;
using Unity.Netcode;
using UnityEngine;

public class Car : NetworkBehaviour
{
    [SerializeField] private InteractableView car;

    private void Awake()
    {
        car.OnInteracted += CarOnOnInteracted;
    }

    private void CarOnOnInteracted()
    {
        InteractedServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractedServerRpc()
    {
        ObjectiveManager.Instance.CompleteTask(0,0);
    }
}