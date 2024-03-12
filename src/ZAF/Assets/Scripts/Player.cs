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
    private float maxHealth;
    //private float currentStamina;
    private Vector2 knockBack;

    CapsuleCollider2D playerCollider;


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
        knockBack = new Vector2(-3f, 0.0f);

        DisplayStats();
        isHit = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(playerState == PlayerState.Dead)
        {
            //When Player is Dead
            Debug.Log("Player has Died");
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    public override void DmgTaken(float damage)
    {
        base.DmgTaken(damage);
        control.inStun = true;
        isHit = true;
        control.rb.AddForce(knockBack, ForceMode2D.Impulse);
    }


    public override void BasicAttack(float dmg)     //RAPEATING CODE HERE CANT THINK OF SOLUTION AT MOOMENT  BESIDES GOING ONE ATTACKPOINT ROUTE
    {
        isAttacking = true;
        //enemy.GetComponent<EnemyPotrol>().DmgTaken(attackDmg);

    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
