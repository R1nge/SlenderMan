using Unity.Netcode;

namespace Game
{
    public class TestView : NetworkBehaviour
    {
        private void Start()
        {
            GetDataServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void GetDataServerRpc(ServerRpcParams rpcParams = default)
        {
            var data = Lobby.Lobby.Instance.GetData(rpcParams.Receive.SenderClientId);
            var name = data.Name;
            var team = data.Team;
            print($"Name: {name}; Team: {team}");
            print($"Length: {Lobby.Lobby.Instance.GetPlayerCount()}");
        }
    }
}