using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    public Material skyboxMaterial;

    [SerializeField] private GameObject menuScreen, lobbyScreen;
    [SerializeField] private TMP_InputField lobbyInput;
    [SerializeField] private TextMeshProUGUI lobbyTitle, lobbyIDText;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Transform playerListContainer;
    [SerializeField] private GameObject playerListItemPrefab;

    private NetworkManager networkManager;

    private void Awake()
    {
        Debug.Log("MainMenuManager Awake called.");
    }

    private void Start()
    {
        Debug.Log("MainMenuManager Start called.");

        RenderSettings.skybox = skyboxMaterial;

        // Initialize networkManager in Start
        networkManager = NetworkManager.singleton;

        if (networkManager == null)
        {
            Debug.LogError("NetworkManager singleton is not set. Ensure there is a NetworkManager in the scene.");
            networkManager = FindObjectOfType<NetworkManager>(); // Attempt to find it in the scene
        }

        if (networkManager != null)
        {
            Debug.Log("NetworkManager singleton is set.");
            OpenMainMenu();
        }
        else
        {
            Debug.LogError("NetworkManager still not found. Ensure it is correctly set in the scene.");
        }
    }

    public void CreateLobby()
    {
        Debug.Log("CreateLobby called.");
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
        Debug.Log("OpenMainMenu called.");
        CloseAllScreens();
        menuScreen.SetActive(true);
    }

    public void OpenLobby()
    {
        Debug.Log("OpenLobby called.");
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
        Debug.Log("JoinLobby called.");
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
        Debug.Log("LeaveLobby called.");
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
        Debug.Log("StartGame called.");
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
        Debug.Log($"LobbyEntered called with lobbyName: {lobbyName}, isHost: {isHost}");
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

    public static void UpdatePlayerList(List<string> playerNames)
    {
        Debug.Log("UpdatePlayerList called.");
        var instance = FindObjectOfType<MainMenuManager>();
        if (instance != null)
        {
            foreach (Transform child in instance.playerListContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var playerName in playerNames)
            {
                var playerListItem = Instantiate(instance.playerListItemPrefab, instance.playerListContainer);
                playerListItem.GetComponentInChildren<TextMeshProUGUI>().text = playerName;
            }
        }
    }
}
