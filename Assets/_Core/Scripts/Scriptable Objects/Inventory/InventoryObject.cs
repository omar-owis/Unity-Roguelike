using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    private const int stackSize = 99;
    public ItemDatabaseObject Database;
    [SerializeField] private int defaultInventorySize;
    public ItemType[] allowableItems;
    public Inventory inventory;

    public InventorySlot[] GetSlots
    {
        get
        {
            return inventory.Slots;
        }
    }

    public int Size { get { return inventory.Slots.Length; } }

    public int DefaultInventorySize { get { return defaultInventorySize; } }

    public bool AddItem(Item item, int amount)
    {
        foreach(ItemType itemType in allowableItems)
        {
            if(itemType == Database.ItemsObjects[item.ID].type)
            {
                if (Database.ItemsObjects[item.ID].isStackable)
                {
                    for (int i = 0; i < GetSlots.Length; i++)
                    {
                        if (GetSlots[i].item.ID == item.ID && GetSlots[i].amount < stackSize)
                        {
                            GetSlots[i].AddAmount(amount);
                            return true;
                        }
                    }
                }

                return AddToEmptySlot(item, amount);
            }
        }

        return false;
    }

    private bool AddToEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.ID <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return true;
            }
        }

        return false; // full inventory
    }

    public void SwapItems(InventorySlot slotObject1, InventorySlot slotObject2)
    {
        if (slotObject2.CanPlaceInSlot(slotObject1) && slotObject1.CanPlaceInSlot(slotObject2))
        {
            InventorySlot temp = new InventorySlot(slotObject2);
            slotObject2.UpdateSlot(slotObject1);
            slotObject1.UpdateSlot(temp);
        }
    }

    //public bool RemoveItem(Item _item)
    //{
    //    for (int i = 0; i < GetSlots.Length; i++)
    //    {
    //        if (GetSlots[i].item == _item)
    //        {
    //            GetSlots[i].RemoveItem();
    //            return true;
    //        }
    //    }
    //    return false; // item not found
    //}

    //public int DecrementAmount(Item _item)
    //{
    //    for (int i = 0; i < GetSlots.Length; i++)
    //    {
    //        if (GetSlots[i].item == _item)
    //        {
    //            if(GetSlots[i].amount > 1)
    //            {
    //                GetSlots[i].addAmount(-1);
    //                return GetSlots[i].amount;
    //            }
    //            else
    //            {
    //                GetSlots[i].UpdateSlot(null, 0);
    //                return 0;
    //            }
    //        }
    //    }
    //    return -1;
    //}
    
    public void UpdateCapacity(int newCapacity)
    {
       Inventory Temp = new Inventory(newCapacity);

        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (i >= newCapacity) break;
            Temp.Slots[i].UpdateSlot(GetSlots[i]);
        }

        SetSize(newCapacity);

        for (int i = 0; i < GetSlots.Length; i++)
        {
            GetSlots[i].UpdateSlot(Temp.Slots[i]);
        }
    }

    public void AddInventorySlots(Inventory _inventory)
    {
        int newInvSize = GetSlots.Length + _inventory.Slots.Length;
        int oldInvSize = GetSlots.Length;
        Inventory Temp = new Inventory(oldInvSize);

        for (int i = 0; i < oldInvSize; i++) Temp.Slots[i].UpdateSlot(GetSlots[i]);

        SetSize(newInvSize);

        for (int i = 0; i < oldInvSize; i++) GetSlots[i].UpdateSlot(Temp.Slots[i]);

        for (int i = oldInvSize; i < newInvSize; i++)
        {
            GetSlots[i].UpdateSlot(_inventory.Slots[i - oldInvSize]);
        }
    }

    public bool SlotHasItem(int slotNum)
    {
        return (GetSlots[slotNum].item.ID > -1);
    }    

    [ContextMenu("Clear")]
    public void Clear()
    {
        inventory = new Inventory(defaultInventorySize);
    }

    public void SetSize(int n)
    {
        inventory = new Inventory(n);
    }
}

[Serializable]
public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[Helper.defaultInvSize];

    public Inventory(int n)
    {
        Slots = new InventorySlot[n];

        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i] = new InventorySlot();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].UpdateSlot(new Item(), 0);
        }
    }
}

public delegate void SlotUpdated(InventorySlot _slot);

[Serializable]
public class InventorySlot
{
    public Item item;
    [NonSerialized] public UserInterface parent;
    [NonSerialized] public GameObject slotDisplay;
    [NonSerialized] public SlotUpdated OnAfterUpdate;
    [NonSerialized] public SlotUpdated OnBeforeUpdate;
    public int amount;


    public ItemObject ItemObject
    {
        get
        {
            if(item.ID >= 0)
            {
                return parent.inventory.Database.ItemsObjects[item.ID];
            }
            return null;
        }
    }


    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(InventorySlot slot)
    {
        UpdateSlot(slot.item, slot.amount);
    }

    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }

    public void UpdateSlot(Item _item, int _amount)
    {
        OnBeforeUpdate?.Invoke(this);
        item = _item;
        amount = _amount;
        OnAfterUpdate?.Invoke(this);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public void UpdateSlot(InventorySlot slot)
    {
        UpdateSlot(slot.item, slot.amount);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public void RemoveAmount(int value)
    {
        if (amount == 1)
        {
            RemoveItem();
        }
        UpdateSlot(item, amount -= value);
    }

    public bool CanPlaceInSlot(InventorySlot slot)
    {
        if (slot.ItemObject == null || slot.ItemObject.data.ID < 0)
            return true;
        if (slot.parent == this.parent)
            return true;

        return false;
    }
}
