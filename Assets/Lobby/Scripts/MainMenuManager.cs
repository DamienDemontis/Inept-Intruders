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

    private void Awake()
    {
        networkManager = NetworkManager.singleton;

        if (networkManager == null)
        {
            Debug.LogError("NetworkManager singleton is not set. Ensure there is a NetworkManager in the scene.");
        }
    }

    private void Start()
    {
        RenderSettings.skybox = skyboxMaterial;

        if (networkManager != null)
        {
            OpenMainMenu();
        }
    }

    public void CreateLobby()
    {
        if (networkManager != null)
        {
            networkManager.StartHost();
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
        string lobbyID = lobbyInput.text;
        if (networkManager != null)
        {
            networkManager.networkAddress = lobbyID;
            networkManager.StartClient();
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
            instance.lobbyTitle.text = lobbyName;
            instance.startGameButton.gameObject.SetActive(isHost);
            instance.lobbyIDText.text = NetworkManager.singleton.networkAddress;
            instance.OpenLobby();
        }
        else
        {
            Debug.LogError("MainMenuManager instance is not found.");
        }
    }
}
