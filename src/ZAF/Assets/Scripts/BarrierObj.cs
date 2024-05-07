using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using static ZombieBehaviour;

public enum BarrierState { fullHealth, InBetween, broken}

public class BarrierObj : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public BarrierState state;

    public float damageTimeout;
    public float takeDamageTimer;

    public bool isBrokenState;
    public bool startTimer;                                                                                                                                   

    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        state = BarrierState.fullHealth;
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer == true)
        {
            takeDamageTimer -= Time.deltaTime; // Decrement the timer
            if(takeDamageTimer <= 0)
            {
                takeDamageTimer = 0;
            }
        }

        switch (state)
        {
            case BarrierState.fullHealth:
                isBrokenState = false;
                if (currentHealth == 0)
                {
                    this.state = BarrierState.broken;
                }
                spriteRenderer.enabled = true;
                boxCollider.enabled = true;
                break;
            case BarrierState.InBetween:
                spriteRenderer.enabled = false;
                break;
            case BarrierState.broken:
                isBrokenState = true;
                spriteRenderer.enabled = false;
                boxCollider.enabled = false;
                startTimer = false;
                break;
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;      
        ResetTimer();
    }

    void ResetTimer()
    {
        takeDamageTimer = damageTimeout;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == BarrierState.broken)
        {
            if (collision.gameObject.tag == "Player")
            {
                state = BarrierState.InBetween;
                boxCollider.enabled = true;
            }
            if (collision.gameObject.tag == "Zombie")
            {
                boxCollider.enabled = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state == BarrierState.InBetween)
        {
            if (collision.gameObject.tag == "Player")
            {
                state = BarrierState.broken;
                boxCollider.enabled = false;
            }
        }
    }
}
