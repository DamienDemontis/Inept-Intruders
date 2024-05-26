using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class MainMenuManager : MonoBehaviour
{
    public Material skyboxMaterial;

    [SerializeField] private GameObject menuScreen, lobbyScreen;
    [SerializeField] private TMP_InputField lobbyInput;

    [SerializeField] private TextMeshProUGUI lobbyTitle, lobbyIDText;
    [SerializeField] private Button startGameButton;

    private NetworkManager networkManager;

    private void Start()
    {
        RenderSettings.skybox = skyboxMaterial;

        // Initialize networkManager in Start
        networkManager = NetworkManager.singleton;

        if (networkManager == null)
        {
            Debug.LogError("NetworkManager singleton is not set. Ensure there is a NetworkManager in the scene.");
        }
        else
        {
            OpenMainMenu();
        }
    }

    public void CreateLobby()
    {
        if (networkManager != null)
        {
            networkManager.StartHost();
            lobbyIDText.text = "127.0.0.1:" + networkManager.GetComponent<TelepathyTransport>().port; // Update the UI with the local address and port
        }
        else
        {
            Debug.LogError("NetworkManager singleton is not set. Cannot create lobby.");
        }
    }

    public void OpenMainMenu()
    {
        CloseAllScreens();
        menuScreen.SetActive(true);
    }

    public void OpenLobby()
    {
        CloseAllScreens();
        lobbyScreen.SetActive(true);
    }

    void CloseAllScreens()
    {
        menuScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }

    public void JoinLobby()
    {
        string networkAddress = lobbyInput.text;
        if (networkManager != null)
        {
            string[] addressParts = networkAddress.Split(':');
            if (addressParts.Length == 2)
            {
                networkManager.networkAddress = addressParts[0];
                ushort port;
                if (ushort.TryParse(addressParts[1], out port))
                {
                    networkManager.GetComponent<TelepathyTransport>().port = port;
                    networkManager.StartClient();
                }
                else
                {
                    Debug.LogError("Invalid port number.");
                }
            }
            else
            {
                Debug.LogError("Invalid network address format. Use IP:Port.");
            }
        }
        else
        {
            Debug.LogError("NetworkManager singleton is not set. Cannot join lobby.");
        }
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            networkManager.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            networkManager.StopClient();
        }
        OpenMainMenu();
    }

    public void StartGame()
    {
        if (networkManager != null)
        {
            networkManager.ServerChangeScene("GameplayScene");
        }
        else
        {
            Debug.LogError("NetworkManager singleton is not set. Cannot start game.");
        }
    }

    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        var instance = FindObjectOfType<MainMenuManager>();
        if (instance != null)
        {
            instance.lobbyTitle.text = "Lobby";
            instance.startGameButton.gameObject.SetActive(isHost);
            instance.OpenLobby();
        }
        else
        {
            Debug.LogError("MainMenuManager instance is not found.");
        }
    }
}
