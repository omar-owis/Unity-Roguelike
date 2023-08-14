using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Object", menuName = "Inventory System/Items/Resource")]
public class ResourceObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Resource;
    }

    public override void EnableEffects(CharacterData c)
    {
    }

    public override void UnenableEffects(CharacterData c)
    {
    }
}
