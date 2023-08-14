using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YesNoItemPrompt : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    private InventorySlot slot;
    //private UserInterface userInterface;

    private void Awake()
    {
        UserInterface.DropInterface += Enable;
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        Disable();
    }

    public void Enable(InventorySlot _slot)
    {
        slot = _slot;

        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void OnDropItemYes()
    {
        slot.RemoveItem();
        Disable();
    }
}
