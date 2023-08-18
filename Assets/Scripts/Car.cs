using System;
using Game.Objectives;
using Items;
using Unity.Netcode;
using UnityEngine;

public class Car : NetworkBehaviour
{
    [SerializeField] private InteractableView fuelTank;
    [SerializeField] private InteractableView battery;

    private void Awake()
    {
        fuelTank.OnInteracted += FuelTankOnOnInteracted;
        battery.OnInteracted += OnBatteryChangedServerRpc;
    }

    private void FuelTankOnOnInteracted()
    {
        OnTankFueledServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnTankFueledServerRpc()
    {
        ObjectiveManager.Instance.CompleteTask(0, Task.TaskType.CarFuel);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnBatteryChangedServerRpc()
    {
        ObjectiveManager.Instance.CompleteTask(0, Task.TaskType.CarBattery);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        fuelTank.OnInteracted -= FuelTankOnOnInteracted;
        battery.OnInteracted -= OnBatteryChangedServerRpc;
    }
}