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

public class Player : Humans
{
    public enum WeaponsActive { _Pistol = 0, _Ak = 1, _MTAR = 2 }
    public WeaponsActive weaponActive;
    

    private PlayerControl control;
    private SpriteRenderer playerSprite;

    public float attackRanf = .6f;
    public bool fixingWall;
    public LayerMask enemyyLayers;
    private bool isAttacking = false;
    public int cuurentWeapon = 1;
    public int currency;
    private int mystreyBoxAmount = 150;
    private int amountToFix = 15;

    CapsuleCollider2D playerCollider;

    public TextMeshProUGUI UI_BarreirText;
    public TextMeshProUGUI UI_CurrencyText;
    public GameObject UI_Restart;
    public Shooting shooting;

    private Pistol pistol;
    private AK ak;
    private MTAR tar;


    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<PlayerControl>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        shooting = GetComponent<Shooting>();
        pistol = GetComponent<Pistol>();
        ak = GetComponent<AK>();
        tar = GetComponent<MTAR>();
        weaponActive = WeaponsActive._Pistol;

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

        UI_CurrencyText.text = currency.ToString();

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
            IncrementWeaponState();
            control.nextWeapon = false;
        }
        if (control.prevWeapon == true)
        {
            DeCrementWeaponState();
            control.prevWeapon = false;
        }
        if (control.isShooting)
        {
            switch (weaponActive)
            {
                case WeaponsActive._Pistol:
                    pistol.enabled = true;
                    pistol.SwitchWeapon(pistol);
                    pistol.Shoot();
                    ak.enabled = false;
                    tar.enabled = false;
                    break;
                case WeaponsActive._Ak:
                    ak.enabled = true;
                    ak.SwitchWeapon(ak);
                    ak.Shoot();
                    tar.enabled = false;
                    pistol.enabled = false;
                    break;
                case WeaponsActive._MTAR:
                    tar.enabled = true;
                    tar.SwitchWeapon(tar);
                    tar.Shoot();
                    ak.enabled= false;
                    pistol.enabled = false;
                    break;

            }

            control.isShooting = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Zombie")
        {
            this.DmgTaken(25);
        }

        if(other.gameObject.tag == "MystryBox")
        {
            UI_BarreirText.text = "Press 'F' to Buy new Weapon for " + mystreyBoxAmount;
            UI_BarreirText.enabled = true;
            if (control.IsInteracting == true)
            {
                if(currency >= mystreyBoxAmount)
                {
                    currency -= mystreyBoxAmount;
                }
                UI_BarreirText.enabled = false;
            }
            control.IsInteracting = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MystryBox")
        {
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
    void IncrementWeaponState()
    {
        int weaponActiveValue = (int)weaponActive; // Convert enum to integer
        weaponActiveValue++; // Increment the integer value

        // Cycle through the enum values in a circular manner
        if (weaponActiveValue >= Enum.GetValues(typeof(WeaponsActive)).Length)
        {
            weaponActiveValue = 0; // Reset to the first enum value if it exceeds the enum length
        }

        weaponActive = (WeaponsActive)weaponActiveValue; // Convert back to enum type
    }
    void DeCrementWeaponState()
    {
        int weaponActiveValue = (int)weaponActive; // Convert enum to integer
        weaponActiveValue--; // Increment the integer value

        // Cycle through the enum values in a circular manner
        if (weaponActiveValue < 0)
        {
            weaponActiveValue = Enum.GetValues(typeof(WeaponsActive)).Length - 1;
        }

        weaponActive = (WeaponsActive)weaponActiveValue; // Convert back to enum type
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
