using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public LevelCount lvls;

    public GameObject enemyObj;
    public float minTimeToSpawn;
    public float maxTimeToSpawn;
    private float timeIntilSpawn;

    public Vector2[] locations;


    // Start is called before the first frame update
    void Start()
    {
        TimeTOSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timeIntilSpawn -= Time.deltaTime;

       
        if(timeIntilSpawn <= 0 && lvls.stopSpawning != true)
        {
            foreach (Vector2 loc in locations)
            {
                Instantiate(enemyObj, loc, Quaternion.identity);
            }
            lvls.Count++;
            TimeTOSpawn();
        }
        if(lvls.zombiesInScene == 0 && lvls.stopSpawning == true)
        {
            StartCoroutine(lvls.WaitForNextRound());
        }

    }
    void TimeTOSpawn()
    {
        timeIntilSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
    }

}
