using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
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

    public float attackDamagePerSecond = 10f;
    public float damageInterval = 100f; // Time interval between damage ticks

    public Rigidbody2D rb;
    public LevelCount lvl;

    // Start is called before the first frame update
    void Start()
    {
        Name = gameObject.name;
        maxHealth = 100;
        currentHealth = maxHealth;
        attackDmg = 50;
        fireDmg = 35;
        barrierDmg = 10;

        moveSpeed = 4f;
        knockBack = new Vector2(-5f, -10f);

        //DisplayStats();
        isHit = false;

        enemyState = EnemyState.chasingBarrier;
        doors = GameObject.FindGameObjectsWithTag("Barrier");
        hubs = GameObject.FindGameObjectsWithTag("Hubs");

        isDestroyingBarrier = false;
    }

    // Update is called once per frame
    void Update()
    {

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
                bool closestDoorIsActive;
                GameObject closestDoor = FindClosestDoor(out closestDoorIsActive);
                if (closestDoorIsActive == false)
                {
                    enemyState = EnemyState.chasingPlayer;
                }
                if (doors.Length > 0)
                {

                    if (closestDoor != null && closestDoorIsActive)
                    {
                        chaseTowardsTarget(closestDoor);
                    }
                }
                break;
            case EnemyState.beakBarrier:
                ///this.rb.AddForce(knockBack, ForceMode2D.Impulse);
                
                isDestroyingBarrier = true;
    
                //enemyState = EnemyState.chasingBarrier;
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
                    Debug.Log("CHASING CLOSEST HUB" + closestHub);
                    chaseTowardsTarget(closestHub);
                }
                break;
        }
        


    }
    GameObject FindClosestDoor(out bool doorIsActive)
    {
        GameObject closestDoor = null;
        float shortestDistance = Mathf.Infinity;

        doorIsActive = false; // Initialize doorIsActive to false

        foreach (GameObject door in doors)
        {
            float distance = Vector3.Distance(transform.position, door.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestDoor = door;

                // Check if the closest door is active
                doorIsActive = closestDoor.activeSelf;
            }
        }

        return closestDoor;
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
            Debug.Log("CHANGE ZOMBIE STATE BACK TO CHASING PLAYER");
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
                //Damage barrier until destoyed before switching to chasing player
                //assigne script to barrier that holds health variable
                BarrierObj barrier = collision.collider.GetComponent<BarrierObj>();
                //barrier.TakeDamage(attackDamagePerSecond * damageInterval);
                // If barrier object and component exist
                if (barrier != null)
                {
                    // Deal damage to the barrier
                    // Start coroutine to deal damage over time
                    StartCoroutine(DealDamageOverTime(barrier));
                    
                }
            }
        }
        if (collision.gameObject.gameObject.tag == "wall")
        {
            isCollidingWithWall = true;
            collisionStartTime++;
            if (collisionStartTime >= 6f)
            {
                enemyState = EnemyState.stuckOnWall;
            }
        }
        if (collision.gameObject.gameObject.tag == "obj" && enemyState == EnemyState.stuckOnWall)
        {
            enemyState = EnemyState.chasingPlayer;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "bullet")//Based on weapon change damage //create system/script that holds damage each weopon does
        {
            //currentHealth -= 55f;
            DmgTaken(55f);
        }
       
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.gameObject.tag == "wall")
        {
            isCollidingWithWall = false;
            collisionStartTime = 0;
            if(enemyState == EnemyState.stuckOnWall)
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
    private System.Collections.IEnumerator DealDamageOverTime(BarrierObj barrier)
    {
        if (barrier.currentHealth > 0)
        {
            
            //isDestroyingBarrier = false;
            // Wait for the next damage interval
            yield return new WaitForSeconds(3f);
            // Deal damage to the barrier
            barrier.TakeDamage(barrierDmg);

        }
        //Debug.Log("BARRIER HEALTH DESTOYED" + barrier.currentHealth);
        isbarrierDestroyed = true;
        enemyState = EnemyState.chasingPlayer;

    }
}
