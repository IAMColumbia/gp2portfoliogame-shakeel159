using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Humans : MonoBehaviour //NO LONGER REFERING TO HUMANS ONLY CHANGE NAME TO DESTROYABLEOBJECTS
{
    public enum PlayerState { Alive, Dead } // states to implement
    public PlayerState playerState;
    public string Name;
    public float currentHealth;
    public float currentStamina;
    public float attackDmg;
    public float fireDmg;
    public bool isHit;
    public float maxHealth;
    //private float currentStamina;
    public Vector2 knockBack;   


    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerState.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerState)
        {
            case PlayerState.Alive:
                break;
            case PlayerState.Dead:
                break;
        }
    }

    public virtual void DisplayStats()
    {
        Debug.Log(this.Name + "has health of " + this.currentHealth);
    }
    public virtual void BasicAttack(float attackDmg)
    {
        throw new NotImplementedException();
    }
    public virtual void DmgTaken(float damage)
    {
        this.currentHealth -= damage;
        //Debug.Log(this.Name + " current health: " + this.currentHealth);
        if(this.currentHealth <= 0)
        {
            currentHealth = 0;
            playerState = PlayerState.Dead;
            Debug.Log("player state: " + playerState);

        }
        else
        {
            playerState = PlayerState.Alive;
        }
    }
}
