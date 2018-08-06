using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCtrl : NetworkBehaviour
{
    [SerializeField] Tank tankPrefab;

    //private int spawnTimer = 0;

    [SyncVar]
    public int playerId = -1;

    [Command]
    public void CmdPlaceUnit(Vector3 position, Quaternion rotation, int OwningPlayer)
    {
        Debug.Assert(OwningPlayer >= 0);

        var tankInst = Instantiate(tankPrefab, position + new Vector3(0, 0.1f, 0), rotation) as Tank;
        tankInst.Player = OwningPlayer;
        tankInst.state = Tank.State.Idle;

        // Spawn the unit on the clients
        NetworkServer.Spawn(tankInst.gameObject);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            PlayerCtrl pc = p.GetComponent<PlayerCtrl>();
            if (pc.playerId != OwningPlayer)
                continue;

            pc.SpendPlayerCredits(Tank.cost);
        }
    }

    [ClientRpc]
    public void RpcUpdateSpawnTimerDisplay(int spawnTimer)
    {
        GameObject HUD = GameObject.FindWithTag("HUD");
        HUD.GetComponent<HUDManager>().UpdateSpawnTimer(spawnTimer);
    }

    [ClientRpc]
    public void RpcEnableCredits()
    {
        GetComponent<Credits>().bCreditCounterEnabled = true;
    }

    public void SpendPlayerCredits(int amount)
    {
        if (!isServer) return;
        GetComponent<Credits>().SpendCredits(amount);
    }


}
