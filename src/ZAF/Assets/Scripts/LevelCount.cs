using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCount : MonoBehaviour
{

    public int currentRound;
    public int Count;
    public int zombiesInScene;
    public bool stopSpawning;
    public int maxZombiesPerRound;

    public GameObject[] xombiesInScene;

    // Start is called before the first frame update
    void Start()
    {
        currentRound = 1;
        stopSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        xombiesInScene = GameObject.FindGameObjectsWithTag("Zombie");
        zombiesInScene = xombiesInScene.Length;

        if(Count == maxZombiesPerRound)
        {
            stopSpawning = true;
        }

    }
    public IEnumerator WaitForNextRound()
    {
        if(stopSpawning == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(5f);

        setUpStats();
    }
    void setUpStats()
    {
        if(zombiesInScene == 0)
        {
            Count = 0;
            currentRound++;
            maxZombiesPerRound++;
            stopSpawning = false;
        }

    }
}
