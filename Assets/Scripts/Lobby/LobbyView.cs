using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyView : NetworkBehaviour
    {
        [SerializeField] private Button start;
        [SerializeField] private Button slender, human;

        private void Awake()
        {
            human.onClick.AddListener(SelectHuman);
            slender.onClick.AddListener(SelectSlender);
            start.onClick.AddListener(StartGame);
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
        }

        private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode mode)
        {
            if (sceneName == "Game" && clientId == NetworkManager.Singleton.LocalClientId)
            {
                UnloadLobby();
            }
        }

        private void SelectHuman()
        {
            SelectHumanServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectHumanServerRpc(ServerRpcParams rpcParams = default)
        {
            var id = rpcParams.Receive.SenderClientId;
            Lobby.Instance.SelectHuman($"Name {id}", id);
        }

        private void SelectSlender()
        {
            SelectSlenderServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectSlenderServerRpc(ServerRpcParams rpcParams = default)
        {
            var id = rpcParams.Receive.SenderClientId;
            Lobby.Instance.SelectSlender($"Name {id}", id);
        }

        private void StartGame()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }

        private void UnloadLobby()
        {
            SceneManager.UnloadSceneAsync("Lobby");
        }
    }
}