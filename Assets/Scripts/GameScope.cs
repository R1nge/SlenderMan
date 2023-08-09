using Game.States;
using VContainer;
using VContainer.Unity;

public class GameScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<StateManager>(Lifetime.Scoped);
    }
}