using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkMgr : NetworkManager
{
    [SerializeField] GameManager gameManager;

    private int m_players = 0;

    public override void OnServerConnect(NetworkConnection conn)
    {
        playerPrefab.GetComponent<PlayerController>().playerId = m_players;

        m_players++;
        Debug.Log("Players Connected: " + m_players);

        if (m_players == 2)
        {
            gameManager.StartGame();
        }
    }
}
