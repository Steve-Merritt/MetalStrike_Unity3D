﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private Color startColor;
    private Renderer RenderComponent;

    public Tank tankPrefab;
    public int OwningPlayer;

    private bool Occupied = false;

    private void Start()
    {
        RenderComponent = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        startColor = RenderComponent.material.color;
        RenderComponent.material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        RenderComponent.material.color = startColor;
    }

    private void OnMouseDown()
    {
        if (!Occupied)
        {
            var tankInst = Instantiate(tankPrefab, transform.position + new Vector3(0, 0.1f, 0), transform.rotation) as Tank;
            tankInst.Player = OwningPlayer;
            tankInst.state = Tank.State.Idle;
            Occupied = true;
        }
    }
}
