using UnityEngine;
using Mirror;
using System.Collections.Generic;


public class CustomNetworkManager : NetworkManager
{
    public GameObject lobbyPlayerPrefab;
    public GameObject gameplayPlayerPrefab;

    private bool isGameplayScene = false;
    public static CustomNetworkManager Instance { get; private set; }

    private void Awake()
    {
        Debug.Log("CustomNetworkManager Awake called.");
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        NetworkClient.OnConnectedEvent += OnClientConnected;
        NetworkClient.OnDisconnectedEvent += OnClientDisconnected;
    }

    public override void OnDestroy()
    {
        NetworkClient.OnConnectedEvent -= OnClientConnected;
        NetworkClient.OnDisconnectedEvent -= OnClientDisconnected;
        base.OnDestroy();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player;
        if (isGameplayScene)
        {
            player = Instantiate(gameplayPlayerPrefab);
        }
        else
        {
            player = Instantiate(lobbyPlayerPrefab);
        }
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public void UpdatePlayerList()
    {
        if (isGameplayScene) return;

        var players = FindObjectsOfType<LobbyPlayer>();
        var playerNames = new List<string>();

        foreach (var player in players)
        {
            playerNames.Add(player.playerName);
        }

        MainMenuManager.UpdatePlayerList(playerNames);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        MainMenuManager.LobbyEntered("Client Lobby", false);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        MainMenuManager instance = FindObjectOfType<MainMenuManager>();
        if (instance != null)
        {
            instance.LeaveLobby();
        }
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        MainMenuManager.LobbyEntered("Host Lobby", true);
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        MainMenuManager instance = FindObjectOfType<MainMenuManager>();
        if (instance != null)
        {
            instance.LeaveLobby();
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        MainMenuManager instance = FindObjectOfType<MainMenuManager>();
        if (instance != null)
        {
            instance.LeaveLobby();
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        if (numPlayers == 0)
        {
            StopHost();
            MainMenuManager instance = FindObjectOfType<MainMenuManager>();
            if (instance != null)
            {
                instance.LeaveLobby();
            }
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        isGameplayScene = (newSceneName == "GameplayScene");
        base.ServerChangeScene(newSceneName);
    }

    private void OnClientConnected()
    {
        MainMenuManager.LobbyEntered("Client Lobby", false);
    }

    private void OnClientDisconnected()
    {
        MainMenuManager instance = FindObjectOfType<MainMenuManager>();
        if (instance != null)
        {
            instance.LeaveLobby();
        }
    }
}
