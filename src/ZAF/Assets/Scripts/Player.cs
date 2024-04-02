using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Humans
{
    private PlayerControl control;
    private SpriteRenderer playerSprite;

    public float attackRanf = .6f;
    public LayerMask enemyyLayers;
    private bool isAttacking = false;

    CapsuleCollider2D playerCollider;

    public GameObject UI_Restart;

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<PlayerControl>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        StartStats();
    }
    public void StartStats()
    {
        Name = gameObject.name;
        maxHealth = 100;
        currentHealth = maxHealth;
        attackDmg = 10;
        fireDmg = 35;
        knockBack = new Vector2(5f, 0.0f);

        //DisplayStats();
        isHit = false;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 playerFacingDirection = control.GetPlayerFacingDirection();

        if (playerState == PlayerState.Dead)
        {
            Time.timeScale = 0f; // Pause time
            UI_Restart.SetActive(true);
        }
        if(playerState == PlayerState.Alive)
        {
            Time.timeScale = 1f; // Pause time
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Zombie")
        {
            DmgTaken(25);
        }
    }

    public override void DmgTaken(float damage)
    {
        base.DmgTaken(damage);
        control.inStun = true;
        isHit = true;
        //control.rb.AddForce(knockBack, ForceMode2D.Impulse);
        
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
