using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tank tankPrefab;
    public Transform team1Spawn;
    public Transform team2Spawn;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void Simulate()
    {
        SpawnTank(1);
        SpawnTank(2);
    }

    public void SpawnTank(int OwningPlayer)
    {
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;

        if (OwningPlayer == 1)
        {
            spawnPosition = team1Spawn.position;
            spawnRotation = team1Spawn.rotation;
        }
        else
        {
            spawnPosition = team2Spawn.position;
            spawnRotation = team2Spawn.rotation;
        }

        var tankInst = Instantiate(tankPrefab, spawnPosition, spawnRotation) as Tank;
        tankInst.Player = OwningPlayer;
        tankInst.state = Tank.State.MovingToTarget;
    }

}
