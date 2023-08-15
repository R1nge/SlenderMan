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

        private void Start()
        {
            if (SteamClient.IsValid)
            {
                host.onClick.AddListener(HostSteam);
                join.onClick.AddListener(() => JoinSteam(ulong.Parse(lobbyId.text)));

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
            else
            {
                host.onClick.AddListener(HostLocal);
                join.onClick.AddListener(JoinLocal);
            }
        }

        private void HostLocal()
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }

        private void JoinLocal()
        {
            NetworkManager.Singleton.StartClient();
        }

        private async void HostSteam()
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            NetworkManager.Singleton.StartHost();
            await SteamMatchmaking.CreateLobbyAsync(4);
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }

        private async void JoinSteam(ulong lobbyId)
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