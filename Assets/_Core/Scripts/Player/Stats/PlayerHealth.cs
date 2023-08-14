using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public static PlayerHealth instance;

    int maxHealth;
    float health;

    public event Action DamageTaken;
    public event Action HealthUpgraded;

    public float Health
    {
        get
        {
            return health;
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
        }
    }

    //private void Start()
    //{
    //    health = maxHealth;
    //}

    public void TakeDamage(float dmg)
    {
        if(health <= 0)
        {
            return;
        }
        health -= dmg;
        if(DamageTaken != null)
        {
            DamageTaken();
        }
    }

    public void Heal(float heal)
    {
        if (health >= maxHealth)
        {
            return;
        }
        health += heal;
        if (DamageTaken != null)
        {
            DamageTaken();
        }
    }

    public void UpgradeHealth(int n)
    {
        maxHealth += n;
        health = maxHealth;
        if(HealthUpgraded != null)
        {
            HealthUpgraded();
        }
    }

    public void SetMaxHealth(int _maxHealth)
    {
        maxHealth = _maxHealth;
        health = maxHealth;
    }
}
