using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInput, ipInput;
        [SerializeField] private Button host, join;
        private MainMenu _mainMenu;

        private void Awake()
        {
            _mainMenu = new MainMenu();
            host.onClick.AddListener(Host);
            join.onClick.AddListener(Join);
            ipInput.onEndEdit.AddListener(SetIp);
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            
            nameInput.onEndEdit.AddListener(OnNameSet);
        }

        //TODO: redo
        private void OnNameSet(string name)
        {
            PlayerPrefs.SetString("Name", name);
            PlayerPrefs.Save();
        }

        private void OnServerStarted()
        {
            _mainMenu.LoadLobby();
        }

        private void Host()
        {
            if (IsValid())
            {
                _mainMenu.Host();
            }
        }

        private void Join()
        {
            if (IsValid())
            {
                _mainMenu.Join();
            }
        }

        private void SetIp(string newIp)
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = newIp;
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(ipInput.text) || string.IsNullOrWhiteSpace(ipInput.text))
            {
                Debug.LogError("MainMenu: ip is not set");
                return false;
            }
            
            if (string.IsNullOrEmpty(nameInput.text) || string.IsNullOrWhiteSpace(nameInput.text))
            {
                Debug.LogError("MainMenu: name is not set");
                return false;
            }

            return true;
        }

        private void OnDestroy()
        {
            host.onClick.RemoveAllListeners();
            join.onClick.RemoveAllListeners();
        }
    }
}