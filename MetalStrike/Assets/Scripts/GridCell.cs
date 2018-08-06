using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GridCell : NetworkBehaviour
{
    private Color startColor;
    private Renderer RenderComponent;

    public Tank tankPrefab;

    [SyncVar]
    public int OwningPlayer;

    private bool Occupied = false;

    private void Start()
    {
        RenderComponent = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        if (!IsOwningPlayer())
            return;

        startColor = RenderComponent.material.color;
        RenderComponent.material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        if (!IsOwningPlayer())
            return;

        RenderComponent.material.color = startColor;
    }

    private void OnMouseDown()
    {
        if (!IsOwningPlayer())
            return;

        GameObject player = GetLocalPlayer();
        PlayerCtrl pc = player.GetComponent<PlayerCtrl>();

        if (!Occupied)
        {
            Debug.Log("player credits " + player.GetComponent<Credits>().credits);
            if (player.GetComponent<Credits>().credits >= Tank.cost)
            {
                pc.CmdPlaceUnit(transform.position, transform.rotation, pc.playerId);
                Occupied = true;
            }
        }
    }

    private bool IsOwningPlayer()
    {
        GameObject pc = GetLocalPlayer();
        return pc.GetComponent<PlayerCtrl>().playerId == OwningPlayer;
    }

    private GameObject GetLocalPlayer()
    {
        List<PlayerController> pcs = NetworkManager.singleton.client.connection.playerControllers;
        return NetworkManager.singleton.client.connection.playerControllers[0].gameObject;
    }
}
