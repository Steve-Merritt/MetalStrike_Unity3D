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
        PlayerController pc = GetLocalPlayerController();
        Debug.Log("player controller " + pc.playerId);

        if (!IsOwningPlayer())
            return;

        if (!Occupied)
        {
            if (true/* || GameManager.credits >= Tank.cost*/)
            {
                //GameManager.credits -= Tank.cost;
                //PlayerController pc = GetLocalPlayerController();
                pc.CmdPlaceUnit(transform.position, transform.rotation, pc.playerId);
                Occupied = true;
            }
        }
    }

    private bool IsOwningPlayer()
    {
        PlayerController pc = GetLocalPlayerController();
        return pc.playerId == OwningPlayer;
    }

    private PlayerController GetLocalPlayerController()
    {
        GameObject player = NetworkManager.singleton.client.connection.playerControllers[0].gameObject;
        return player.GetComponent<PlayerController>();
    }
}
