using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackZombies : MonoBehaviour
{
    public int round;
    public bool hasRoundEnded;

    public GameObject[] xombiesInScene;
    public int Count;

    // Start is called before the first frame update
    void Start()
    {
        round = 1;
    }

    // Update is called once per frame
    void Update()
    {
        xombiesInScene = GameObject.FindGameObjectsWithTag("Zombie");
        Count = xombiesInScene.Length;
        if (xombiesInScene.Length == 0 && !hasRoundEnded)
        {
            hasRoundEnded = true;
            round += AddRound();
        }
    }
    int AddRound()
    {
        return 1;
    }
}
