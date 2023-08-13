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
            var name = PlayerPrefs.GetString("Name", "NN");
            SelectHumanServerRpc(name);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectHumanServerRpc(string name, ServerRpcParams rpcParams = default)
        {
            var id = rpcParams.Receive.SenderClientId;
            Lobby.Instance.SelectHuman(name, id);
        }

        private void SelectSlender()
        {
            var name = PlayerPrefs.GetString("Name", "NN");
            SelectSlenderServerRpc(name);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectSlenderServerRpc(string name, ServerRpcParams rpcParams = default)
        {
            var id = rpcParams.Receive.SenderClientId;
            Lobby.Instance.SelectSlender(name, id);
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