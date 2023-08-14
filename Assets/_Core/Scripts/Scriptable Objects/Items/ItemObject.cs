using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Trinket,
    PowerUp,
    Resource,
    BackPack
}
public abstract class ItemObject : ScriptableObject
{
    public Item data = new Item();
    public Sprite UIDisplay;
    public ItemType type;
    public bool isStackable = false;
    [TextArea(15, 20)]
    public string description;

    public abstract void EnableEffects(CharacterData c);
    public abstract void UnenableEffects(CharacterData c);
}

[Serializable]
public class Item
{
    public string Name;
    public int ID = -1;

    public Item()
    {
        Name = "";
        ID = -1;
    }

    public Item(ItemObject item)
    {
        Name = item.name;
        ID = item.data.ID;
    }
}
