using System;
using Game.Objectives;
using Game.States;
using Items;
using Unity.Netcode;
using UnityEngine;

public class Car : NetworkBehaviour
{
    [SerializeField] private InteractableView fuelTank;
    [SerializeField] private InteractableView battery;

    private void Awake()
    {
        ObjectiveManager.Instance.OnObjectiveComplete += OnCarRepairedServerRpc;
        fuelTank.OnInteracted += FuelTankOnOnInteracted;
        battery.OnInteracted += OnBatteryChangedServerRpc;
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnCarRepairedServerRpc(int obj)
    {
        StateManager.Instance.ChangeState(StateManager.States.EndGame);
    }

    private void FuelTankOnOnInteracted()
    {
        OnTankFueledServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnTankFueledServerRpc()
    {
        ObjectiveManager.Instance.CompleteTask(0, Task.TaskType.CarFuel);
        OnTankFueledClientRpc();
    }

    [ClientRpc]
    private void OnTankFueledClientRpc()
    {
        if (IsServer) return;
        ObjectiveManager.Instance.CompleteTask(0, Task.TaskType.CarFuel);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnBatteryChangedServerRpc()
    {
        ObjectiveManager.Instance.CompleteTask(0, Task.TaskType.CarBattery);
        OnBatteryChangedClientRpc();
    }

    [ClientRpc]
    private void OnBatteryChangedClientRpc()
    {
        if (IsServer) return;
        ObjectiveManager.Instance.CompleteTask(0, Task.TaskType.CarBattery);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        fuelTank.OnInteracted -= FuelTankOnOnInteracted;
        battery.OnInteracted -= OnBatteryChangedServerRpc;
    }
}