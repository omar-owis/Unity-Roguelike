using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject inventorySlotPrefab;

    private void OnEnable()
    {
        UpdateInventoryCapacity();
    }
    private void OnDisable()
    {
    }

    public override void CreateInventorySlots()
    {
        for (int i = 0; i < inventory.inventory.Slots.Length; i++)
        {
            DisplaySlot(i);
        }
    }
    public void UpdateInventoryCapacity()
    {
        DeleteAllSlots();
        CreateInventorySlots();
        UpdateInventorySlots();
    }

    //private Vector3 GetPosition(int n)
    //{
    //    return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (n % NUM_OF_COLUMNS)),
    //        Y_START + (-Y_SPACE_BETWEEN_ITEMS * (n / NUM_OF_COLUMNS)), 0);
    //}

    public void DisplaySlot(int i)
    {
        inventory.inventory.Slots[i].parent = this;
        inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;

        InventorySlot slot = inventory.inventory.Slots[i];
        var obj = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, transform);
        AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
        AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
        AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
        AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
        AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
        inventory.GetSlots[i].slotDisplay = obj;

        slotsOnInterface.Add(obj, slot);
    }

    private void DeleteSlot(int i)
    {
        GameObject objToDelete = this.transform.GetChild(i + 1).gameObject;
        slotsOnInterface[objToDelete].OnAfterUpdate -= OnSlotUpdate;
        slotsOnInterface.Remove(objToDelete);
        Destroy(objToDelete);
    }

    private void DeleteAllSlots()
    {
        for(int i = 0; i < this.transform.childCount - 1; i++)
        {
            DeleteSlot(i);
        }
    }
}
