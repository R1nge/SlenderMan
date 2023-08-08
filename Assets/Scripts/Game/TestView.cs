using Unity.Netcode;
using VContainer;

namespace Game
{
    public class TestView : NetworkBehaviour
    {
        private Lobby.Lobby _lobby;
        
        [Inject]
        private void Construct(Lobby.Lobby lobby)
        {
            _lobby = lobby;
        }

        private void Start()
        {
            GetDataServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDataServerRpc()
        {
            print(_lobby.GetData(0).Name);
            print(_lobby.GetData(1).Name);
        }
    }
}