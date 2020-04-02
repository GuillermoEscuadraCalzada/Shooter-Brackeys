using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
   
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Vida del objeto: " + health);
        if(health <= 0f)
        {
            
            if (GetComponent<Destructible>())
            {
                Destructible destructible = GetComponent<Destructible>();
                destructible.enabled = true;
                destructible.Start();
                destructible.Destroyed();
            }
        }
    }
}
