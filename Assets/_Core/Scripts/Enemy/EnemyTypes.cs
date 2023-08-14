using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTypes
{
    public enum type {SplittingCreature, Default};
    public type enemyType = type.Default;

    public void SetTypeToTag(string tag)
    {
        if(tag == "SplittingCreature")
        {
            enemyType = type.SplittingCreature;
        }

        else
        {
            enemyType = type.Default;
        }
    }
}
