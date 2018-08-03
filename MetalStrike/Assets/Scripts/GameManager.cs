using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public Tank tankPrefab;
    public GridCell gridCellPrefab;

    public Transform[] playerGridOrigin;
    public Transform[] teamSpawn;

    private const int GRID_SIZE = 8;
    private GridCell[] grid = new GridCell[GRID_SIZE* GRID_SIZE];

    public GameObject spawnTimer;

    private Text spawnTimerText;
    [SyncVar]
    string syncSpawnTime;

    public float spawnInterval = 30;
    private float timeUntilNextSpawn = 0;

    public GameObject creditDisplay;
    private Text creditDisplayText;
    public static int credits = 0;
    public int creditIncrement = 10;
    private float timeUntilNextCredits = 1;

    public bool bSpawnTimerEnabled = false;

    // Use this for initialization
    void Start ()
    {
        spawnTimerText = spawnTimer.GetComponent<Text>();
        timeUntilNextSpawn = spawnInterval;

        creditDisplayText = creditDisplay.GetComponent<Text>();
	}

    private void UpdateSpawnTimer()
    {
        timeUntilNextSpawn = Mathf.Clamp(timeUntilNextSpawn -= Time.deltaTime, 0, spawnInterval);
        if (timeUntilNextSpawn <= 0)
        {
            timeUntilNextSpawn = spawnInterval;
            SpawnNextWaves();
        }
        int displayedTime = (int)timeUntilNextSpawn + 1;
        displayedTime = Mathf.Clamp(displayedTime, 1, (int)spawnInterval);
        syncSpawnTime = displayedTime.ToString("F0");
        spawnTimerText.text = syncSpawnTime;
    }

    private void UpdateCredits()
    {
        timeUntilNextCredits = Mathf.Clamp(timeUntilNextCredits -= Time.deltaTime, 0, 1);
        if (timeUntilNextCredits <= 0)
        {
            timeUntilNextCredits = 1;
            credits += creditIncrement;
        }
        creditDisplayText.text = credits.ToString();
    }

    // Update is called once per frame
    void Update ()
    {
        if (bSpawnTimerEnabled)
        {
            UpdateSpawnTimer();
        }

        //UpdateCredits();
    }

    public void SpawnNextWaves()
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
                float dz = teamSpawn[OwningPlayer].position.z - playerGridOrigin[OwningPlayer].position.z;
                Vector3 spawnPosition = t.transform.position;
                spawnPosition.z += dz;

                Quaternion spawnRotation = teamSpawn[OwningPlayer].rotation;

                var tankInst = Instantiate(tankPrefab, spawnPosition, spawnRotation) as Tank;
                tankInst.Player = OwningPlayer;
                tankInst.state = Tank.State.MovingToTarget;
                tankInst.goal = tankInst.transform.position + (tankInst.transform.forward * 50);
                tankInst.velocity = tankInst.transform.forward;

                NetworkServer.Spawn(tankInst.gameObject);
            }
        }

    }

    public void SpawnGrids()
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
                    NetworkServer.Spawn(grid[g].gameObject);
                    ++h;
                }
                ++v;
            }
        }
    }

}
