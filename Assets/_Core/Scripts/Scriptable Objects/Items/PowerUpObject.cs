using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp Object", menuName = "Inventory System/Items/PowerUp")]
public class PowerUpObject : ItemObject
{
    public List<PowerUpItemEffect> effects;
    private void Awake()
    {
        type = ItemType.PowerUp;
        isStackable = true;
    }

    public override void EnableEffects(CharacterData c)
    {
        foreach (PowerUpItemEffect effect in effects)
        {
            effect.ExecuteEffect(this, c);
        }
    }

    public override void UnenableEffects(CharacterData c)
    {
    }
}
