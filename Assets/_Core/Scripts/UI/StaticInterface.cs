using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;

    private void Awake()
    {
        if (slots.Length != inventory.Size) Debug.LogError("Inventory size does not match slot gameobjects");
    }
    public override void CreateInventorySlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.inventory.Slots.Length; i++)
        {
            inventory.inventory.Slots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;

            GameObject obj = slots[i];
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, inventory.inventory.Slots[i]);
        }

    }
}
