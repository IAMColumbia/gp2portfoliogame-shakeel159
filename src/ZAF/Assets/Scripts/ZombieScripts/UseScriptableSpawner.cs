using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UseScriptableSpawner : MonoBehaviour
{
    public ScriptableSpawner spawnerData;

    public int currentlySpawned = 0;
    public float timeBetween = 0;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3 spawnPoint in spawnerData.spawnPoints)
        {
            GameObject spawnedObject = Instantiate(spawnerData.spawnerLoc, transform.position + spawnPoint, Quaternion.identity);
            spawnedObject.transform.SetParent(transform); // Make the spawner the parent of the spawned object
            spawnedObjects.Add(spawnedObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeBetween += 1;

        if (timeBetween >= spawnerData.interval && currentlySpawned < spawnerData.maxSpawnNumber)
        {
            currentlySpawned++;
            timeBetween = 0;
            Instantiate(spawnerData.zombiePrefab, transform.position, transform.rotation);
        }
    }
}
