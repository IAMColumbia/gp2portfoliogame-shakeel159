using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Pistol : Shooting, IWeapon
{

    protected override void Start()
    {
        bulletForce = 15f; // You can adjust this value as needed

        isWeaponActive = true;
        shotRateMilliseconds = .5f;
        shotRateMillisocondsTimer = .5f;
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

    }
    public void Test()
    {
        Debug.Log("PISTOLAAAAAA");
    }
    override public void Shoot()
    {
      
        base.Shoot();
    }
}
