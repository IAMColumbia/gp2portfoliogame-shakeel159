using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Debug = UnityEngine.Debug;

public class ZombieBehaviour : Humans
{
    public enum EnemyState { chasingBarrier, beakBarrier, chasingPlayer, stuckOnWall}
    public EnemyState enemyState;

    private GameObject target;
    public float moveSpeed;
    private bool isCollidingWithWall;
    public float collisionStartTime;

    Vector3 directionToPlayer;
    public GameObject[] doors;
    public GameObject[] hubs;

    private bool isDestroyingBarrier;
    private bool isbarrierDestroyed;
    private float barrierDmg;
    public float barrierTimer;


    public Rigidbody2D rb;
    public LevelCount lvls;

    // Start is called before the first frame update
    void Start()
    {
        Name = gameObject.name;
        maxHealth = 100;
        currentHealth = maxHealth;
        attackDmg = 50;
        fireDmg = 35;
        barrierDmg = 50;

        moveSpeed = 3f;

        //DisplayStats();
        isHit = false;

        enemyState = EnemyState.chasingBarrier;
        doors = GameObject.FindGameObjectsWithTag("Barrier");
        hubs = GameObject.FindGameObjectsWithTag("Hubs");

        // Find the GameObject with the LevelCount script attached to it
        GameObject levelCountObject = GameObject.Find("LevelManager");

        lvls = levelCountObject.GetComponent<LevelCount>();
        isDestroyingBarrier = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(lvls.currentRound == 5)
        {
            moveSpeed = 4;
        }
        if(lvls.currentRound == 10)
        {
            moveSpeed = 5;
        }
        switch (playerState)
        {
            case PlayerState.Alive:
                if (currentHealth <= 0)
                {
                    Destroy(this.gameObject);
                }
                break;
            case PlayerState.Dead:
                Destroy(gameObject);
                break;
        }

        switch (enemyState)
        {
            case EnemyState.chasingBarrier:
                FindClosestDoor();
                BarrierObj otherScript = target.GetComponent<BarrierObj>();
                isbarrierDestroyed = otherScript.isBrokenState;
                if (isbarrierDestroyed == false)
                {
                    chaseTowardsTarget(target);
                }
                else
                {
                    enemyState = EnemyState.chasingPlayer;
                }
                break;
            case EnemyState.beakBarrier:
                ///this.rb.AddForce(knockBack, ForceMode2D.Impulse);
                FindClosestDoor();
                BarrierObj _barrier = target.GetComponent<BarrierObj>();
                barrierTimer = _barrier.takeDamageTimer;
                isDestroyingBarrier = true;
                isbarrierDestroyed = _barrier.isBrokenState;
                if (barrierTimer == 0)
                {
                    DealDamageOverTime(_barrier);
                }
                if (isbarrierDestroyed == true)
                {
                    enemyState = EnemyState.chasingPlayer;
                }
                break;
            case EnemyState.chasingPlayer:
                target = GameObject.FindGameObjectWithTag("Player");
                //Vector3 playerPosition = target.transform.position;
                chaseTowardsTarget(target);
                break;
            case EnemyState.stuckOnWall:
                // move zombie along side wall
                bool findingClosestHub = true;
                GameObject closestHub = FindClosestHub(out findingClosestHub);
                if (findingClosestHub == true)
                {
                    chaseTowardsTarget(closestHub);
                }
                break;
        }
        


    }
    void FindClosestDoor()
    {
        GameObject[] barrierObjects = GameObject.FindGameObjectsWithTag("Barrier");
        float closestDistance = Mathf.Infinity;
        foreach (GameObject barrier in barrierObjects)
        {
            float distance = Vector3.Distance(transform.position, barrier.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = barrier;
            }
        }
    }
    GameObject FindClosestHub(out bool isOnWall)
    {
        GameObject clestestHub = null;
        float shortestDistance = Mathf.Infinity;

        isOnWall = false; // Initialize doorIsActive to false

        foreach (GameObject hub in hubs)
        {
            float distance = Vector3.Distance(transform.position, hub.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                clestestHub = hub;

                // Check if the closest door is active
                isOnWall = clestestHub.activeSelf;
            }
        }

        return clestestHub;
    }

    void chaseTowardsTarget(GameObject obj)
    {
        if (this.transform.position == obj.transform.position)
        {
            enemyState = EnemyState.chasingPlayer;
        }
        // Calculate direction towards the closest door
        Vector3 targt = obj.transform.position - transform.position;

        // Normalize the direction vector to have a magnitude of 1
        targt.Normalize();

        // Move the enemy towards the closest door
        transform.Translate(targt * moveSpeed * Time.deltaTime);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.gameObject.tag == "Barrier")
        {
            enemyState = EnemyState.beakBarrier;
            if (isDestroyingBarrier == true)
            {
                BarrierObj barrier = collision.collider.GetComponent<BarrierObj>();
                
                if (isbarrierDestroyed == false)
                {
                    barrier.startTimer = true;    
                    
                }
            }
        }
        if (collision.gameObject.gameObject.tag == "wall")
        {
            isCollidingWithWall = true;
            collisionStartTime += Time.deltaTime;
            if (collisionStartTime >= 6f)
            {
                enemyState = EnemyState.stuckOnWall;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player pl = Object.FindObjectOfType<Player>(); 
        if (collision.collider.gameObject.tag == "bullet")//Based on weapon change damage //create system/script that holds damage each weopon does
        {
            //currentHealth -= 55f;
            pl.currency += 15;
            DmgTaken(55f);
        }
       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.gameObject.tag == "wall")
        {
            isCollidingWithWall = false;
            collisionStartTime = 0;
            if (enemyState == EnemyState.stuckOnWall)
            {
                enemyState = EnemyState.chasingPlayer;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemyState == EnemyState.stuckOnWall)
        {
            enemyState = EnemyState.chasingPlayer;
        }
    }
    private void DealDamageOverTime(BarrierObj barrier)
    {
        if (barrier.currentHealth > 0)
        {
            barrier.TakeDamage(barrierDmg);
        }
        //Debug.Log("BARRIER HEALTH DESTOYED" + barrier.currentHealth);
        enemyState = EnemyState.chasingPlayer;

    }
}
