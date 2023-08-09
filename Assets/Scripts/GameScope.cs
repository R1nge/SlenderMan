using Game;
using Game.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameScope : LifetimeScope
{
    [SerializeField] private PlayerSpawner playerSpawner;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(playerSpawner);
        builder.Register<StateManager>(Lifetime.Scoped);
    }
}