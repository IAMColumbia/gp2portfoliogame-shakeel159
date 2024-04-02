using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zombie_Spawner", menuName = "Data/ZombieSpawner", order = 1)]
public class ScriptableSpawner : ScriptableObject
{
    public string objectName = "New Spawner";
    public int maxSpawnNumber;

    public float interval = 100f;

    public GameObject zombiePrefab;
    public GameObject spawnerLoc;
    public Vector3[] spawnPoints = new Vector3[1] { Vector3.zero };
}
