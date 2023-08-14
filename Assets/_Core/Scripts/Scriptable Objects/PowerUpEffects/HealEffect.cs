using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect Object", menuName = "Inventory System/Items/Effects/Heal")]
public class HealEffect : PowerUpItemEffect
{
    public float HealAmount;

    public override void ExecuteEffect(PowerUpObject item, CharacterData c)
    {
        c.Heal(HealAmount);
    }
}
