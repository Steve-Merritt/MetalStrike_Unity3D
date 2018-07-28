using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBase : MonoBehaviour {

    private Health health;
    private AudioManager audioManager;
    private ParticleManager particleManager;

    // Use this for initialization
    void Start () {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        particleManager = GameObject.Find("ParticleManager").GetComponent<ParticleManager>();

        health = GetComponent<Health>();
        health.currentHealth = health.maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        // Check vitals
        if (health.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        audioManager.PlayTankExplodeAtLocation(transform.position);
        particleManager.PlayTankExplodeAtLocation(transform.position);
        Destroy(gameObject);
    }
}
