using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ZombieBehaviour : Humans
{
    public enum EnemyState { chasingBarrier, beakBarrier, chasingPlayer }
    public EnemyState enemyState;

    private GameObject target;
    public float moveSpeed;

    Vector3 directionToPlayer;
    public GameObject[] doors;

    private bool isDestroyingBarrier;
    private bool isbarrierDestroyed;
    private float barrierDmg;

    public float attackDamagePerSecond = 10f;
    public float damageInterval = 10f; // Time interval between damage ticks

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
        knockBack = new Vector2(0f, 0f);

        //DisplayStats();
        isHit = false;

        enemyState = EnemyState.chasingBarrier;
        doors = GameObject.FindGameObjectsWithTag("Barrier");

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
                        //BasicAttack(barrierDmg);
                        isDestroyingBarrier = true;
                        if (isbarrierDestroyed == true)
                        {
                            isDestroyingBarrier = false;
                            enemyState = EnemyState.chasingPlayer;
                        }
                        break;
                    case EnemyState.chasingPlayer:
                        target = GameObject.FindGameObjectWithTag("Player");
                        //Vector3 playerPosition = target.transform.position;
                        chaseTowardsTarget(target);
                        break;
                }
                break;
            case PlayerState.Dead:
                Destroy(gameObject);
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

    void chaseTowardsTarget(GameObject obj)
    {
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
                // If barrier object and component exist
                if (barrier != null)
                {
                    // Deal damage to the barrier
                    // Start coroutine to deal damage over time
                    StartCoroutine(DealDamageOverTime(barrier));
                    if(barrier.currentHealth <= 0)
                    {
                        isbarrierDestroyed = true;
                    }
                }
            }
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
    private System.Collections.IEnumerator DealDamageOverTime(BarrierObj barrier)
    {
        while (true)
        {
            // Deal damage to the barrier
            barrier.TakeDamage(attackDamagePerSecond * damageInterval);

            // Wait for the next damage interval
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
