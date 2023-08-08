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
        private void GetDataServerRpc(ServerRpcParams rpcParams = default)
        {
            var data = _lobby.GetData(rpcParams.Receive.SenderClientId);
            var name = data.Name;
            var team = data.Team;
            print($"Name: {name}; Team: {team}");
        }
    }
}