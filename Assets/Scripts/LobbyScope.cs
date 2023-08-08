using VContainer;
using VContainer.Unity;

public class LobbyScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<Lobby.Lobby>(Lifetime.Singleton);
    }
}