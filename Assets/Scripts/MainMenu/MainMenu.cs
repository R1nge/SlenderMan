using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu
    {
        public void Host()
        {
            NetworkManager.Singleton.StartHost();
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        public void Join()
        {
            NetworkManager.Singleton.StartClient();
            SceneManager.UnloadSceneAsync("MainMenu");
        }

        public void LoadLobby()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
        }
    }
}