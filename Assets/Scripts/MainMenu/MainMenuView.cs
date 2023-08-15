using Netcode.Transports.Facepunch;
using Steamworks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField lobbyId;
        [SerializeField] private Button host, join;
        private Steamworks.Data.Lobby _lobby;

        private void Awake()
        {
            host.onClick.AddListener(Host);
            join.onClick.AddListener(() => Join(ulong.Parse(lobbyId.text)));
        }

        private void Start()
        {
            SteamMatchmaking.OnLobbyCreated += (result, lobby) =>
            {
                _lobby = lobby;
                Debug.LogError(result == Result.OK ? "Lobby created" : "Failed to create a lobby");
                Debug.LogError("Game Started");
                _lobby.SetPublic();
                _lobby.SetJoinable(true);
                _lobby.SetGameServer(_lobby.Owner.Id);
                Debug.LogError(_lobby.Id);
                Debug.LogError(_lobby.Owner.Id);
            };

            SteamMatchmaking.OnLobbyEntered += lobby =>
            {
                if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
                {
                    return;
                }

                _lobby = lobby;

                Debug.LogError("Entered the lobby");
                Help();
            };

            SteamMatchmaking.OnLobbyMemberJoined += (lobby, friend) => { Debug.LogError("Entered the lobby"); };

            SteamMatchmaking.OnLobbyInvite += (friend, lobby) => { Debug.LogError($"Invited {friend.Name}"); };
        }

        private async void Host()
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            NetworkManager.Singleton.StartHost();
            await SteamMatchmaking.CreateLobbyAsync(4);
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }

        private async void Join(ulong lobbyId)
        {
            Debug.LogError("TRYING TO JOIN");
            await SteamMatchmaking.JoinLobbyAsync(lobbyId);
            Debug.LogError("JOINED");
        }

        private void Help()
        {
            NetworkManager.Singleton.GetComponent<FacepunchTransport>().targetSteamId = _lobby.Owner.Id;
            NetworkManager.Singleton.StartClient();
            SceneManager.UnloadSceneAsync("MainMenu");
            Debug.LogError("Started a client");
        }

        private void OnDestroy()
        {
            host.onClick.RemoveAllListeners();
            join.onClick.RemoveAllListeners();
        }
    }
}