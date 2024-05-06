using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Player;

public class Player : Humans
{

    private PlayerControl control;
    private SpriteRenderer playerSprite;

    public float attackRanf = .6f;
    public bool fixingWall;
    public LayerMask enemyyLayers;
    public bool isAttacking = false;
    public int currency;
    public int mystreyBoxAmount = 150;
    private int amountToFix = 15;
    public bool startTimer;
    public bool inMenu;

    CapsuleCollider2D playerCollider;

    public TextMeshProUGUI UI_BarreirText;
    public GameObject UI_Restart;
    public TextMeshProUGUI UI_AmmoCount;

    private Shoot_Weapon InstantatieBulletScript;


    public Scriptable_WeaponBase pistol;
    public Scriptable_WeaponBase Ak;
    public Scriptable_WeaponBase mTar;
    

    public List<Scriptable_WeaponBase> weaponsAvilable;

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<PlayerControl>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        InstantatieBulletScript = GetComponent<Shoot_Weapon>();

        weaponsAvilable = new List<Scriptable_WeaponBase>()// make it so that player has one weapon and progress to gain new ones over time 
        {
            pistol, 
            //Ak,
            //mTar
        };
        InstantatieBulletScript.CurrentWeapon = pistol;
        StartStats();
    }
    public void StartStats()
    {
        Name = gameObject.name;
        maxHealth = 100;
        currentHealth = maxHealth;
        attackDmg = 10;
        fireDmg = 35;
        //DisplayStats();
        isHit = false;
        UI_BarreirText.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 playerFacingDirection = control.GetPlayerFacingDirection();

        UI_AmmoCount.text = InstantatieBulletScript.CurrentWeapon.cuurentAmmo.ToString();

        if (playerState == PlayerState.Dead)
        {
            Time.timeScale = 0f; // Pause time
            UI_Restart.SetActive(true);
        }
        if(playerState == PlayerState.Alive)
        {
            Time.timeScale = 1f; // Pause time
        }

        if (control.nextWeapon == true)
        {
            NextWeapon();
            control.nextWeapon = false;
        }
        if (control.prevWeapon == true)
        {
            PrevWeapon();
            control.prevWeapon = false;
        }


        if (control.isShooting)
        {
            InstantatieBulletScript.Fire();
            control.isShooting = false;
        }
        if(control.isReloading)
        {
            if (currency > 50)
            {
                InstantatieBulletScript.StartReload();
                currency -= 50;
                control.isReloading = false;
            }
        }
    }

    public void NextWeapon()
    {
        int weaponIndex = weaponsAvilable.IndexOf(InstantatieBulletScript.CurrentWeapon);
        weaponIndex++;
        if (weaponIndex >= weaponsAvilable.Count) weaponIndex = 0;
        InstantatieBulletScript.CurrentWeapon = weaponsAvilable[weaponIndex];
    }
    public void PrevWeapon()
    {
        int weaponIndex = weaponsAvilable.IndexOf(InstantatieBulletScript.CurrentWeapon);
        weaponIndex--;
        if (weaponIndex <= weaponsAvilable.Count) weaponIndex = 0;
        InstantatieBulletScript.CurrentWeapon = weaponsAvilable[weaponIndex];
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Zombie")
        {
            this.DmgTaken(25);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MystryBox")
        {
            UI_BarreirText.text = "Press 'F' to open";
            UI_BarreirText.enabled = true;
            if (control.IsInteracting == true)
            {
                inMenu = true;

            }
            //control.IsInteracting = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MystryBox")
        {
            inMenu = false;
            UI_BarreirText.enabled = false;
            control.IsInteracting = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        BarrierObj barrier = collision.GetComponent<BarrierObj>();

        if (collision.gameObject.tag == "Barrier")
        {

            if (barrier.state == BarrierState.broken || barrier.state == BarrierState.InBetween)
            {
                UI_BarreirText.text = "Press 'F' to Fix For " + amountToFix;
                UI_BarreirText.enabled = true;
                if (control.IsInteracting == true && currency >= amountToFix)
                {
                    barrier.currentHealth = barrier.maxHealth;
                    barrier.state = BarrierState.fullHealth;
                    UI_BarreirText.enabled = false;
                }
                control.IsInteracting = false;
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            UI_BarreirText.enabled = false;
            control.IsInteracting = false;
        }
    }
    public override void DmgTaken(float damage)
    {
        base.DmgTaken(damage);
        isHit = true;
        
    }
    public override void BasicAttack(float dmg)     
    {
        isAttacking = true;

    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
