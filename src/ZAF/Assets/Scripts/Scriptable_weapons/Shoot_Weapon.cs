using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shoot_Weapon : MonoBehaviour
{

   // https://www.youtube.com/watch?v=kXbQMhwj5Uc //Shooting logic
    public Scriptable_WeaponBase CurrentWeapon;  
    public Transform firePoint;
    public GameObject bulletPrefab;
    public TextMeshProUGUI reloadText;

    float timeSinceLastShot;

    // Start is called before the first frame update
    void Start()
    {
        CurrentWeapon.cuurentAmmo = CurrentWeapon.magSize;
        CurrentWeapon.isrealloding = false;
        reloadText.enabled = false;
    }
    public void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if(CurrentWeapon.cuurentAmmo == 0 )
        {
            reloadText.enabled = true;
        }
        else
        {
            reloadText.enabled = false;
        }

        if (CurrentWeapon.name == "Pistol")
        {
            CurrentWeapon.cuurentAmmo = CurrentWeapon.magSize;
        }
    }
    private bool CanShoot() => !CurrentWeapon.isrealloding && timeSinceLastShot > 1f / (CurrentWeapon.fireRate / 60f);

    public void Fire()
    {
        if(CurrentWeapon.cuurentAmmo > 0)
        {
            if (CurrentWeapon.name == "Mtar")
            {
                if (CanShoot())
                {
                    StartCoroutine(SpawnBullets());
                }
            }
            else
            {

                if (CanShoot())
                {
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                    rb.AddForce(-firePoint.up * CurrentWeapon.bulletForce, ForceMode2D.Impulse);
                    CurrentWeapon.cuurentAmmo--;
                    timeSinceLastShot = 0;
                }
            }
        }
        
    }
    IEnumerator SpawnBullets()
    {
        for (int i = 0; i < CurrentWeapon.burstCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(-firePoint.up * CurrentWeapon.bulletForce, ForceMode2D.Impulse);
            CurrentWeapon.cuurentAmmo--;
            timeSinceLastShot = 0;
            yield return new WaitForSeconds(.1f); // Adjust delay as needed
        }
    }
    private bool BurstShoot()
    {
        if (CurrentWeapon.currentBurstCount >= CurrentWeapon.burstCount)
            return false;

        return true;
    }
    public void StartReload()
    {
        CurrentWeapon.isrealloding = true;
        CurrentWeapon.cuurentAmmo = CurrentWeapon.magSize;

        CurrentWeapon.isrealloding = false;
    }
   
}
