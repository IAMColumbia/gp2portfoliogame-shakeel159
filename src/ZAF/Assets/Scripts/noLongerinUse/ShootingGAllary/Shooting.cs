using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public interface IWeapon
{
    void Shoot();
}

public class Shooting : MonoBehaviour , IWeapon
{
    public IWeapon currentWeapon;
    // Public property to access currentWeapon
    PlayerControl playerControl;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public bool isWeaponActive;
    public float bulletForce;
    public bool _isShooting;
    public float shotRateMilliseconds,shotRateMillisocondsTimer,limiteShotRate;


    public float limitShotRate
    {
        get { return limiteShotRate; }
        set
        {
            limiteShotRate = value;
            shotRateMilliseconds = limiteShotRate * 1000;
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerControl = GetComponent<PlayerControl>();
    }
    //THIS CLASS WILL TAKE INFORMATIO NFROM GUNSTASK(PROBABLY) AND INSTANTATIE BULLETS BASED ON TAHT
    //RHAR MEANS SHOOT() METHOD WONT SIMPLY SHOOT WHEN ON CLICK BUT NEEDS TO LOGIC THAT FIRES BASED IN INPUTS GIVEN
    // Update is called once per frame
    protected virtual void Update()
    {
        shotRateMillisocondsTimer -= Time.deltaTime;
        if (_isShooting && shotRateMillisocondsTimer <= 0)
        {
            //// Shoot if shooting is allowed based on the timer
            Shoot();
            _isShooting = false;
            // Reset the timer
            shotRateMillisocondsTimer = shotRateMilliseconds;
        }
    }
    public virtual void SwitchWeapon(IWeapon newWeapon)
    {
        currentWeapon = newWeapon;
    }

    public virtual void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(-firePoint.up * bulletForce, ForceMode2D.Impulse);
        // Reset the shooting timer
        shotRateMillisocondsTimer = shotRateMilliseconds;

    }

}
