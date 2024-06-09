using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    public GameObject lobbyPlayerPrefab;
    public GameObject robertPlayerPrefab;
    public GameObject camGuyPlayerPrefab;

    public Transform[] robertSpawnPoints;
    public Transform[] camGuySpawnPoints;

    private bool isGameplayScene = false;
    private bool isSceneChanging = false;
    private int playerCount = 1;
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

    public int GetNextPlayerNumber()
    {
        return playerCount++;
    }

    public override void OnDestroy()
    {
        NetworkClient.OnConnectedEvent -= OnClientConnected;
        NetworkClient.OnDisconnectedEvent -= OnClientDisconnected;
        base.OnDestroy();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (isGameplayScene)
        {
            var lobbyPlayer = conn.identity.GetComponent<LobbyPlayer>();
            if (lobbyPlayer != null)
            {
                GameObject playerPrefab;
                Transform spawnPoint;

                if (lobbyPlayer.selectedCharacter == "Cam Guy")
                {
                    playerPrefab = camGuyPlayerPrefab;
                    spawnPoint = GetSpawnPoint(camGuySpawnPoints);
                }
                else
                {
                    playerPrefab = robertPlayerPrefab;
                    spawnPoint = GetSpawnPoint(robertSpawnPoints);
                }

                var player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
                NetworkServer.AddPlayerForConnection(conn, player);
            }
        }
        else
        {
            var lobbyPlayer = Instantiate(lobbyPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
        }
        LobbyManager.Instance.UpdatePlayerList();
    }

    private Transform GetSpawnPoint(Transform[] spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (!spawnPoint.gameObject.activeInHierarchy)
            {
                spawnPoint.gameObject.SetActive(true);
                return spawnPoint;
            }
        }
        return spawnPoints[0]; // Default fallback
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        MainMenuManager.LobbyEntered("Client Lobby", false);
        LobbyManager.Instance.UpdatePlayerList();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        MainMenuManager instance = FindObjectOfType<MainMenuManager>();
        if (instance != null)
        {
            instance.LeaveLobby();
        }
        LobbyManager.Instance.UpdatePlayerList();
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        MainMenuManager.LobbyEntered("Host Lobby", true);
        LobbyManager.Instance.UpdatePlayerList();
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
        LobbyManager.Instance.UpdatePlayerList();
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (isSceneChanging)
        {
            Debug.LogError("Scene change is already in progress for " + newSceneName);
            return;
        }

        isSceneChanging = true;
        isGameplayScene = (newSceneName == "R1 - Intro");
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        isSceneChanging = false;  // Reset the flag when the scene change is complete
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
