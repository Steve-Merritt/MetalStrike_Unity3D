using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tank : NetworkBehaviour
{
    [SerializeField] float Speed = 10;
    [SerializeField] float AttackRange = 5;
    [SerializeField] float FireRate = 2;
    [SerializeField] int AttackDamage = 10;

    private Transform ShellSpawnPoint;
    private Transform HealthbarTransform;

    public ParticleSystem fireBurstPrefab;    
    private ParticleManager particleManager;

    [SyncVar]
    public int Player = 0;

    public enum State {  Idle, MovingToTarget, Attacking };

    [SyncVar]
    public State state;

    private GameObject EnemyTarget;

    private float FiringTimer = 0.0f;

    public AudioClip audioClip_TankFire;
    private AudioManager audioManager;

    public Vector3 velocity;

    [SyncVar]
    public Vector3 goal;

    [SyncVar]
    private Vector3 goalDirection;

    private Health health;

    public static int cost = 50;

    // Use this for initialization
    void Start ()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        particleManager = GameObject.Find("ParticleManager").GetComponent<ParticleManager>();

        goalDirection = transform.forward;

        // Assign ourselves to the healthbar
        health = GetComponent<Health>();
        health.currentHealth = health.maxHealth;

        // Adjust healthbar to face camera
        HealthbarTransform =  transform.Find("HealthbarCanvas");
        HealthbarTransform.rotation = Quaternion.Euler(45*transform.forward.z, transform.rotation.eulerAngles.y-45, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Check vitals
        if (health.currentHealth <= 0 || IsOutOfBounds())
        {
            Die();
        }

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
            // Continue to reload while moving
            if (FiringTimer > 0.0f)
            {
                FiringTimer -= Time.deltaTime;
                FiringTimer = Mathf.Clamp(FiringTimer, 0.0f, FireRate);
            }

            Vector3 avoid = Avoidance();
            Vector3 tendToGoal = TendToGoal();

            velocity += avoid + tendToGoal;
            velocity.Normalize();

            transform.rotation = Quaternion.LookRotation(velocity);
            transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
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
                if (nearestAlly.transform.position.z > transform.position.z)
                {
                    avoid = Vector3.right;
                }
            }
            else
            {
                if (nearestAlly.transform.position.z < transform.position.z)
                {
                    avoid = Vector3.right;
                }
            }
        }

        return avoid;
    }

    private Vector3 TendToGoal()
    {
        Vector3 tendToGoal = Vector3.zero;

        tendToGoal = goalDirection / 40;

        return tendToGoal;
    }

    private void Attack()
    {
        if (EnemyTarget != null)
        {
            // rotate to face enemy
            Vector3 enemyDirection = EnemyTarget.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(enemyDirection);

            // fire if ready!
            if (FiringTimer <= 0.0f)
            {
                PlayFireBurst();
                particleManager.PlayFireHitAtLocation(EnemyTarget.transform.position);
                EnemyTarget.GetComponent<Health>().TakeDamage(AttackDamage);
                FiringTimer = FireRate;
            }
            else
            {
                FiringTimer -= Time.deltaTime;
                FiringTimer = Mathf.Clamp(FiringTimer, 0.0f, FireRate);
            }
        }
        else
        {
            state = State.MovingToTarget;
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

        if (EnemyTarget == null)
        {
            // No Enemy tanks found - see if we're in range of the enemy base
            GameObject enemyBase = null;
            if (Player == 0)
            {
                enemyBase = GameObject.FindGameObjectWithTag("Team2Base");
            }
            else if (Player == 1)
            {
                enemyBase = GameObject.FindGameObjectWithTag("Team1Base");
            }

            if (enemyBase)
            {
                var range = Vector3.Distance(enemyBase.transform.position, transform.position);
                if (range < AttackRange)
                {
                    nearestEnemyRange = range;
                    EnemyTarget = enemyBase;
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

        ShellSpawnPoint = transform.Find("ShellSpawn");

        ParticleSystem ps = Instantiate(fireBurstPrefab, ShellSpawnPoint.position, ShellSpawnPoint.rotation) as ParticleSystem;
        Destroy(ps.gameObject, ps.main.startLifetime.constant);

        AudioSource.PlayClipAtPoint(audioClip_TankFire, transform.position);
    }

    public void Die()
    {
        audioManager.PlayTankExplodeAtLocation(transform.position);
        particleManager.PlayTankExplodeAtLocation(transform.position);
        Destroy(gameObject);
    }

    private bool IsOutOfBounds()
    {
        if (state != State.Idle && (goal.z - transform.position.z) * goalDirection.z < 0)
        {
            return true;
        }

        return false;
    }

}
