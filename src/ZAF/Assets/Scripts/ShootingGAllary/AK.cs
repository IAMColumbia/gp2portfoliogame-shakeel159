using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class AK : Shooting, IWeapon
{
    protected override void Start()
    {
        bulletForce = 15f; // You can adjust this value as needed

        isWeaponActive = true;
        shotRateMilliseconds = .1f;
        shotRateMillisocondsTimer = 1f;
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

    }

    public override void Shoot()
    {
        base.Shoot();
    }
}
