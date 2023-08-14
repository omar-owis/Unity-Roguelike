//using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New BackPack Object", menuName = "Inventory System/Items/BackPack")]
public class BackPackObject : ItemObject
{
    public Inventory Inventory = new Inventory(5);
    
    private void Awake()
    {
        type = ItemType.BackPack;
    }

    public override void EnableEffects(CharacterData c)
    {
    }

    public override void UnenableEffects(CharacterData c)
    {
    }

    private void OnEnable()
    {
        Inventory.Clear();
    }

}