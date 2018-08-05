using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] Tank tankPrefab;

    public int playerId = -1;

    [Command]
    public void CmdPlaceUnit(Vector3 position, Quaternion rotation, int OwningPlayer)
    {
        Debug.Assert(OwningPlayer >= 0);

        Debug.Log("Spawning Tank for Player " + OwningPlayer);
        var tankInst = Instantiate(tankPrefab, position + new Vector3(0, 0.1f, 0), rotation) as Tank;
        tankInst.Player = OwningPlayer;
        tankInst.state = Tank.State.Idle;

        // Spawn the unit on the clients
        NetworkServer.Spawn(tankInst.gameObject);
    }
}
