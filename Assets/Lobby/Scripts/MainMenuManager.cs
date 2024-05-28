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
            instance.UpdatePlayerList(); // Update the player list when entering the lobby
        }
        else
        {
            Debug.LogError("MainMenuManager instance is not found.");
        }
    }

    public void UpdatePlayerList(List<string> playerNames = null, List<string> selectedCharacters = null)
    {
        Debug.Log("UpdatePlayerList called.");
        if (playerNames == null || selectedCharacters == null)
        {
            var players = FindObjectsOfType<LobbyPlayer>();
            playerNames = new List<string>();
            selectedCharacters = new List<string>();
            foreach (var player in players)
            {
                playerNames.Add(player.playerName);
                selectedCharacters.Add(player.selectedCharacter);
            }
        }

        foreach (Transform child in playerListContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerNames.Count; i++)
        {
            var playerListItem = Instantiate(playerListItemPrefab, playerListContainer);
            playerListItem.GetComponentInChildren<TextMeshProUGUI>().text = playerNames[i];

            var player = FindPlayerByName(playerNames[i]);

            if (player != null)
            {
                // Character selection buttons
                var buttons = playerListItem.GetComponentsInChildren<Button>();
                var camGuyButton = buttons[0];
                var robertButton = buttons[1];

                // Clear previous listeners to prevent duplicate actions
                camGuyButton.onClick.RemoveAllListeners();
                robertButton.onClick.RemoveAllListeners();

                // Set up listeners to only affect the local player
                if (player.isLocalPlayer)
                {
                    camGuyButton.onClick.AddListener(() => {
                        player.CmdSelectCharacter("Cam Guy");
                    });

                    robertButton.onClick.AddListener(() => {
                        player.CmdSelectCharacter("Robert");
                    });
                }

                // Update button visuals based on selection
                if (selectedCharacters[i] == "Cam Guy")
                {
                    camGuyButton.image.color = Color.green;
                    robertButton.image.color = Color.white;
                }
                else
                {
                    camGuyButton.image.color = Color.white;
                    robertButton.image.color = Color.green;
                }
            }
        }
    }

    private LobbyPlayer FindLocalPlayer()
    {
        var players = FindObjectsOfType<LobbyPlayer>();
        foreach (var player in players)
        {
            if (player.isLocalPlayer)
            {
                return player;
            }
        }
        return null;
    }

    private LobbyPlayer FindPlayerByName(string playerName)
    {
        var players = FindObjectsOfType<LobbyPlayer>();
        foreach (var player in players)
        {
            if (player.playerName == playerName)
            {
                return player;
            }
        }
        return null;
    }
}
