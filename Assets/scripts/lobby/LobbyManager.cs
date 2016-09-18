﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager
{
    public static LobbyManager instance;

    public Text ip;
    public RectTransform startPanel;
    public RectTransform stopPanel;
    public RectTransform lobbyPanel;
    public delegate void QuitRoomDelegate();
    public QuitRoomDelegate quitRoomDelegate;

    private GameMode mode;
    public GameMode Mode
    {
        get
        {
            return mode;
        }
        private set
        {
            if ((mode = value) == GameMode.ClassicSingle)
            {
                minPlayers = 1;
            }
            else
            {
                minPlayers = 2;
            }
        }
    }

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnLobbyServerPlayersReady()
    {
        base.OnLobbyServerPlayersReady();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        ShowLobbyGUI();
        stopPanel.localScale = new Vector3(0, 0, 0);
    }

    public void ShowLobbyGUI()
    {
        startPanel.localScale = stopPanel.localScale = lobbyPanel.localScale = new Vector3(1, 1, 1);
    }

    public void HideLobbyGUI()
    {
        startPanel.localScale = stopPanel.localScale = lobbyPanel.localScale = new Vector3(0, 0, 0);
    }

    public void CreateRoom()
    {
        Mode = GameMode.ClassicDouble;
        StartHost();
        startPanel.localScale = new Vector3(0, 1, 1);
        stopPanel.localScale = new Vector3(1, 1, 1);
        quitRoomDelegate = QuitHostRoom;
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(ip.text))
        {
            Mode = GameMode.ClassicDouble;
            NetworkClient client = StartClient();
            client.Connect(ip.text, networkPort);
            startPanel.localScale = new Vector3(0, 1, 1);
            stopPanel.localScale = new Vector3(1, 1, 1);
            quitRoomDelegate = QuitRemoteRoom;
        }
    }

    public void SingleGame()
    {
        Mode = GameMode.ClassicSingle;
        StartHost();
        quitRoomDelegate = QuitHostRoom;
    }

    public void QuitHostRoom()
    {
        StopHost();
        startPanel.localScale = new Vector3(1, 1, 1);
        stopPanel.localScale = new Vector3(0, 1, 1);
    }

    public void QuitRemoteRoom()
    {
        StopClient();
        startPanel.localScale = new Vector3(1, 1, 1);
        stopPanel.localScale = new Vector3(0, 1, 1);
    }

    public void QuitRoom()
    {
        quitRoomDelegate();
    }

    public RectTransform leftContainer;
    public RectTransform rightContainer;

    private LobbyPlayer leftPlayer;
    private LobbyPlayer rightPlayer;

    public void AddPlayer(LobbyPlayer player)
    {
        if (leftPlayer == null)
        {
            leftPlayer = player;
            player.transform.SetParent(leftContainer);
            player.transform.localPosition = new Vector3(0, 0, 0);
            player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (rightPlayer == null)
        {
            rightPlayer = player;
            player.transform.SetParent(rightContainer);
            player.transform.localPosition = new Vector3(0, 0, 0);
            player.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void RemovePlayer(LobbyPlayer player)
    {
        if (leftPlayer != null && leftPlayer.Equals(player))
        {
            leftPlayer = null;
        }
        else if (rightPlayer != null && rightPlayer.Equals(player))
        {
            rightPlayer = null;
        }
    }
}