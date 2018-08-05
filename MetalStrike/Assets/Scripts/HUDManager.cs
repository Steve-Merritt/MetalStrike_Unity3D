using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HUDManager : NetworkBehaviour
{
    public GameObject spawnTimer;
    private Text spawnTimerText;

    public GameObject creditDisplay;
    private Text creditDisplayText;

    void Start()
    {
        spawnTimerText = spawnTimer.GetComponent<Text>();
        creditDisplayText = creditDisplay.GetComponent<Text>();
    }

    [ClientRpc]
    public void RpcUpdateSpawnTimer(int value)
    {
        spawnTimerText.text = value.ToString();
    }

    [ClientRpc]
    public void RpcUpdateCredits(int value)
    {
        creditDisplayText.text = value.ToString();
    }
}
