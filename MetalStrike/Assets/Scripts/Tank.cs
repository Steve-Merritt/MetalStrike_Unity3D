﻿using System.Collections;
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

    public Vector3 velocity;
    public Vector3 goal;

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
            Vector3 avoid = Avoidance();
            Vector3 tendToGoal = TendToGoal();

            velocity += avoid + tendToGoal;
            velocity.Normalize();

            transform.Translate(velocity * Speed * Time.deltaTime);
        }
    }

    private Vector3 Avoidance()
    {
        Vector3 avoid = Vector3.zero;

        // find nearest ally
        Tank nearestAlly = FindNearestAlly();
        if (nearestAlly != this)
        {
            // determine if they are in front of us
            // TODO: find a better way to do this
            if (Player == 0)
            {
                if (nearestAlly.transform.position.x > transform.position.x)
                {
                    avoid = Vector3.forward;
                }
            }
            else
            {
                if (nearestAlly.transform.position.x < transform.position.x)
                {
                    avoid = Vector3.forward;
                }
            }
        }

        return avoid;
    }

    private Vector3 TendToGoal()
    {
        Vector3 tendToGoal = Vector3.zero;

        tendToGoal = Vector3.right / 50;

        return tendToGoal;
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

    private Tank FindNearestAlly()
    {
        float nearestAllyRange = 1.0f;
        Tank nearestAlly = this; // start with ourselves as the nearest ally

        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        foreach (GameObject go in tanks)
        {
            Tank t = go.GetComponent<Tank>();
            if (t == this)
                continue;

            if (!IsEnemy(t.Player) && t.state != State.Idle)
            {
                var range = Vector3.Distance(t.transform.position, transform.position);
                if (range < nearestAllyRange)
                {
                    nearestAllyRange = range;
                    nearestAlly = t.GetComponent<Tank>();
                }
            }
        }

        return nearestAlly;
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

        //Vector3 position = transform.position + Vector3.right * -0.4f + Vector3.up * 0.4f; TODO: adjust hit effect
        ParticleSystem ps = Instantiate(fireHitPrefab, transform.position, Quaternion.identity) as ParticleSystem;
        Destroy(ps.gameObject, ps.main.startLifetime.constant);
    }

}
