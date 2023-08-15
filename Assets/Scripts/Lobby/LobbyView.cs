using Steamworks;
using TMPro;
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

        private void Start()
        {
            start.gameObject.SetActive(NetworkObject.IsOwner);
        }

        private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode mode)
        {
            if (sceneName == "Game" && clientId == NetworkManager.Singleton.LocalClientId)
            {
                UnloadLobby();
            }

            if (IsServer && sceneName != "Game")
            {
                var humanCount = Lobby.Instance.HumanCount();
                UpdateHumanCountClientRpc(humanCount);
                var slenderCount = Lobby.Instance.SlenderCount();
                UpdateSlenderCountClientRpc(slenderCount);
            }
        }

        private void SelectHuman()
        {
            string name;

            if (SteamClient.IsValid)
            {
                name = SteamClient.Name ?? $"{Random.Range(0, 111100101)}";
            }
            else
            {
                name = $"{Random.Range(0, 111100101)}";
            }

            SelectHumanServerRpc(name);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectHumanServerRpc(string name, ServerRpcParams rpcParams = default)
        {
            var id = rpcParams.Receive.SenderClientId;
            Lobby.Instance.SelectHuman(name, id);
            var humanCount = Lobby.Instance.HumanCount();
            UpdateHumanCountClientRpc(humanCount);
            var slenderCount = Lobby.Instance.SlenderCount();
            UpdateSlenderCountClientRpc(slenderCount);
        }

        [ClientRpc]
        private void UpdateHumanCountClientRpc(int count)
        {
            human.GetComponentInChildren<TextMeshProUGUI>().text = $"Human {count}";
        }

        private void SelectSlender()
        {
            string name;

            if (SteamClient.IsValid)
            {
                name = SteamClient.Name ?? $"{Random.Range(0, 111100101)}";
            }
            else
            {
                name = $"{Random.Range(0, 111100101)}";
            }

            SelectSlenderServerRpc(name);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SelectSlenderServerRpc(string name, ServerRpcParams rpcParams = default)
        {
            var id = rpcParams.Receive.SenderClientId;
            Lobby.Instance.SelectSlender(name, id);
            var humanCount = Lobby.Instance.HumanCount();
            UpdateHumanCountClientRpc(humanCount);
            var slenderCount = Lobby.Instance.SlenderCount();
            UpdateSlenderCountClientRpc(slenderCount);
        }

        [ClientRpc]
        private void UpdateSlenderCountClientRpc(int count)
        {
            slender.GetComponentInChildren<TextMeshProUGUI>().text = $"Slender {count}";
        }

        private void StartGame()
        {
            //TODO: start game only when everyone have chosen a role
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }

        private void UnloadLobby()
        {
            SceneManager.UnloadSceneAsync("Lobby");
        }
    }
}