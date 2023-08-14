using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public delegate void TooltipEvent(GameObject obj, InventoryObject inventory, int id);
public delegate void ItemPrompt(InventorySlot slot);

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;

    public static event TooltipEvent ToolTip;
    public static event ItemPrompt DropInterface;

    protected Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    private bool isToolTipActive = false;

    public abstract void CreateInventorySlots();

    private void Start()
    {
        CreateInventorySlots();

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    protected void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.ID >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = _slot.ItemObject.UIDisplay;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    public void UpdateInventorySlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in slotsOnInterface)
        {
            if(_slot.Value.item.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = _slot.Value.ItemObject.UIDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        if (slotsOnInterface[obj].item.ID >= 0 && !isToolTipActive)
        {
            ToolTip?.Invoke(obj, inventory, slotsOnInterface[obj].item.ID);
            isToolTipActive = true;
        }


        InventoryMouseData.slotHoverOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        if (slotsOnInterface[obj].item.ID >= 0 && isToolTipActive)
        {
            ToolTip?.Invoke(obj, inventory, slotsOnInterface[obj].item.ID);
            isToolTipActive = false;
        }

        InventoryMouseData.slotHoverOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        GameObject tempItem = null; 


        if (slotsOnInterface[obj].item.ID >= 0)
        {
            tempItem = new GameObject();
            RectTransform rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
            tempItem.transform.SetParent(transform.parent); // FIX: set parent to canvas rather than parent
            UnityEngine.UI.Image img = tempItem.AddComponent<UnityEngine.UI.Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.UIDisplay;
            img.raycastTarget = false;
        }

        InventoryMouseData.tempItemDragged = tempItem;
    }
    public void OnDrag(GameObject obj)
    {
        if (InventoryMouseData.tempItemDragged != null) 
            InventoryMouseData.tempItemDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }
    public void OnDragEnd(GameObject obj)
    {
        Destroy(InventoryMouseData.tempItemDragged);
        if(InventoryMouseData.mouseOverUI == null)
        {
            DropInterface?.Invoke(slotsOnInterface[obj]);
            return;
        }
        if(InventoryMouseData.slotHoverOver)
        {
            InventorySlot mouseHoverSlotData = InventoryMouseData.mouseOverUI.slotsOnInterface[InventoryMouseData.slotHoverOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
            if (mouseHoverSlotData.item.ID >= 0 && !isToolTipActive)
            {
                ToolTip?.Invoke(InventoryMouseData.slotHoverOver, inventory, mouseHoverSlotData.item.ID);
                isToolTipActive = true;
            }
        }
    }
    public void OnEnterInterface(GameObject obj)
    {
        InventoryMouseData.mouseOverUI = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        InventoryMouseData.mouseOverUI = null;
    }
}

public static class InventoryMouseData
{
    public static UserInterface mouseOverUI;
    public static GameObject tempItemDragged;
    public static GameObject slotHoverOver;
}