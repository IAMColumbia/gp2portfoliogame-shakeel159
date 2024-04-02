using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_spawner : MonoBehaviour
{
    //TRACK ENEMIES IN SCENE
    public int maxSpawnNumber;
    int currentlySpawned;
    // spawner behaviour
    public float interval = 100;
    float timeBetween;
    //reference of Object to instantaite
    public GameObject zombiePrefab;
    //GET LOCATION OF SPAWNER TO SPAWN
    GameObject spawner;
    Vector2 spawnerLoc;
    public TrackZombies TrackZombies;
    int currentRound;

    public List<GameObject> spawnedZombies = new List<GameObject>();

    void Start()
    {
        currentlySpawned = 0;
        timeBetween = 0;

        currentRound = TrackZombies.round;
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        timeBetween += 1;
        if (timeBetween >= interval && currentlySpawned <= maxSpawnNumber)
        {
            currentlySpawned += 1;
            timeBetween = 0;
            GameObject newZombie = Instantiate(zombiePrefab, transform.position, transform.rotation);
            spawnedZombies.Add(newZombie);
        }
        if(TrackZombies.round != currentRound)
        {
            maxSpawnNumber += 1;
        }
        //if (TrackZombies.round != 1 || TrackZombies.round != currentRound)
        //{
            
        //}

    }
}
