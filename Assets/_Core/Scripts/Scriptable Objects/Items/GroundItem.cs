using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;
    public int amount = 1;

    public void SetItem(ItemObject _item)
    {
        item = _item;
    }

    private void OnValidate()
    {
        if (item.isStackable) amount = Mathf.Clamp(amount, 1, 99);
        else amount = 1;
    }
}
