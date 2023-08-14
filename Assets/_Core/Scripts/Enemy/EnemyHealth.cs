using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    private float health, maxHealth = 20;
    //public EnemyTypes enemyType;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //enemyType.SetTypeToTag(gameObject.tag);
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            TypeEnemyDie();
        }
    }


    void TypeEnemyDie()
    {
        //if(enemyType.enemyType == EnemyTypes.type.Default)
        //{
        //    Destroy(gameObject);
        //}
        if (gameObject.transform.localScale.x / 2 > 0.4)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject minion = Instantiate(gameObject);
                minion.transform.localScale = gameObject.transform.localScale / 2;
            }
        }

        Destroy(gameObject);
    }

}
