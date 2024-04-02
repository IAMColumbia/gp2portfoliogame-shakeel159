using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using static ZombieBehaviour;

public class BarrierObj : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //if(currentHealth <= 0)
        //{
        //    this.gameObject.SetActive(false);
        //    //GameObject.Destroy(this.gameObject);
        //}

    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //Destroy(gameObject);
            this.gameObject.SetActive(false);
        }
    }


}
