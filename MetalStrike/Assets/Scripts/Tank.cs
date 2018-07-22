using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] float Speed = 10;
    [SerializeField] float AttackRange = 5;
    [SerializeField] float FireRate = 2;

    private Transform ShellSpawnPoint;

    public ParticleSystem fireBurstPrefab;
    public ParticleSystem fireHitPrefab;

    public int Player { get; set; }

    public enum State {  Idle, MovingToTarget, Attacking };
    public State state;

    private GameObject EnemyTarget;

    private float FiringTimer = 0.0f;

    AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (Player == 2)
        //    return; // short circuit team 2 spawns for now.

        switch (state)
        {
            case State.Idle:
                break;
            case State.MovingToTarget:
                MoveToTarget();
                break;
            case State.Attacking:
                Attack();
                break;
            default:
                break;
        }
    }

    bool IsEnemy(int OwningPlayer)
    {
        return OwningPlayer != Player;
    }

    private void MoveToTarget()
    {
        FindNearestEnemy();

        if (state == State.MovingToTarget)
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        if (EnemyTarget.activeInHierarchy)
        {
            if (FiringTimer <= 0.0f)
            {
                PlayFireBurst();
                EnemyTarget.GetComponent<Tank>().PlayFireHit();
                FiringTimer = FireRate;
            }
            else
            {
                FiringTimer -= Time.deltaTime;
            }
        }
    }

    private void FindNearestEnemy()
    {
        float nearestEnemyRange = 10000.0f;

        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        foreach (GameObject t in tanks)
        {
            if (IsEnemy(t.GetComponent<Tank>().Player))
            {
                var range = Vector3.Distance(t.transform.position, transform.position);
                if (range < AttackRange && range < nearestEnemyRange)
                {
                    nearestEnemyRange = range;
                    EnemyTarget = t;
                }
            }
        }

        if (EnemyTarget != null && EnemyTarget.activeInHierarchy)
        {
            state = State.Attacking;
        }
    }

    private void PlayFireBurst()
    {
        if (fireBurstPrefab == null) return;

        ShellSpawnPoint = transform.Find("turret/ShellSpawn");

        ParticleSystem ps = Instantiate(fireBurstPrefab, ShellSpawnPoint.position, ShellSpawnPoint.rotation) as ParticleSystem;
        Destroy(ps.gameObject, ps.main.startLifetime.constant);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PlayFireHit()
    {
        if (fireHitPrefab == null) return;

        Vector3 position = transform.position + Vector3.right * -0.4f + Vector3.up * 0.4f;
        ParticleSystem ps = Instantiate(fireHitPrefab, position, Quaternion.identity) as ParticleSystem;
        Destroy(ps.gameObject, ps.main.startLifetime.constant);
    }

}
