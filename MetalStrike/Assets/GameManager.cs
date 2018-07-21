using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform tankPrefab;
    public Transform spawnOriginPrefab;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();		
	}

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnTank();
        }
    }

    private void SpawnTank()
    {
        Instantiate(tankPrefab, spawnOriginPrefab.position, Quaternion.identity);
    }

}
