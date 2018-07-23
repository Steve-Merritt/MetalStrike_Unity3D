using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tank tankPrefab;
    public GridCell gridCellPrefab;

    public Transform[] playerGridOrigin;
    public Transform[] teamSpawn;

    private const int GRID_SIZE = 8;
    private GridCell[] grid = new GridCell[GRID_SIZE* GRID_SIZE];

    // Use this for initialization
    void Start ()
    {
        SpawnGrids();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void Simulate()
    {
        for (int i = 0; i < playerGridOrigin.Length; i++)
        {
            SpawnWave(i);
        }
    }

    public void SpawnWave(int OwningPlayer)
    {
        // spawn tanks relative to grid position
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        foreach (GameObject go in tanks)
        {
            Tank t = go.GetComponent<Tank>();
            if (t.Player == OwningPlayer && t.state == Tank.State.Idle)
            {
                // get position relative to team spawn
                float dx = teamSpawn[OwningPlayer].position.x - playerGridOrigin[OwningPlayer].position.x;
                Vector3 spawnPosition = t.transform.position;
                spawnPosition.x += dx;

                Quaternion spawnRotation = teamSpawn[OwningPlayer].rotation;

                var tankInst = Instantiate(tankPrefab, spawnPosition, spawnRotation) as Tank;
                tankInst.Player = OwningPlayer;
                tankInst.state = Tank.State.MovingToTarget;
                tankInst.goal = tankInst.transform.position + (tankInst.transform.right * 50);
                tankInst.velocity = Vector3.right;
            }
        }

    }

    private void SpawnGrids()
    {
        // generate grids for each player
        int g = 0;
        float spacing = 1.1f;
        for (int i = 0; i < playerGridOrigin.Length; i++)
        {
            float h = 0;
            float v = 0;

            for (int j = 0; j < GRID_SIZE; j++)
            {
                for (int k = 0; k < GRID_SIZE; k++)
                {
                    Vector3 position = playerGridOrigin[i].position + Vector3.forward * (h % GRID_SIZE) * spacing + Vector3.right * v * spacing;
                    grid[g] = Instantiate(gridCellPrefab, position, playerGridOrigin[i].rotation);
                    grid[g].OwningPlayer = i;
                    ++h;
                }
                ++v;
            }
        }
    }

}
