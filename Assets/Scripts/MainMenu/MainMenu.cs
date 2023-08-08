using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenu
    {
        public void Host()
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            NetworkManager.Singleton.StartHost();
        }

        public void Join()
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            NetworkManager.Singleton.StartClient();
        }

        public void LoadLobby()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }
}