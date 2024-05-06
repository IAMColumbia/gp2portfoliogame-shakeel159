using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptable Weapon Data/weapons data")]
public class Scriptable_WeaponBase : ScriptableObject
{

    public new string name;

    public int bulletForce;
    public int cuurentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public bool isrealloding;

    public int burstCount; // Number of shots per burst
    public float burstFireRate; // Time between bursts
    public int currentBurstCount; // Current shots fired in the current burst
    // Add other properties as needed


}