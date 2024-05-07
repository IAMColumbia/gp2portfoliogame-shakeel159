using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class MTAR : Shooting, IWeapon
{
    PlayerControl playerControl;


    protected override void Start()
    {
        bulletForce = 20f; // You can adjust this value as needed

        isWeaponActive = true;
        shotRateMilliseconds = .9f;
        shotRateMillisocondsTimer = 1f;

        playerControl = GetComponent<PlayerControl>();
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
