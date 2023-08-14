using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public Vector2 offset;
    StringBuilder sb;

    private void Awake()
    {
        UserInterface.ToolTip += Disable;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UserInterface.ToolTip += Disable;
        UserInterface.ToolTip -= Enable;
    }

    private void OnDisable()
    {
        UserInterface.ToolTip -= Disable;
        UserInterface.ToolTip += Enable;
    }

    public void Enable(GameObject obj, InventoryObject inventory, int id)
    {
        Vector3 pos = new Vector3(obj.transform.position.x - Screen.width / 2,
            obj.transform.position.y - Screen.height / 2, 0);

        if(pos.x - this.GetComponent<RectTransform>().sizeDelta.x / 2 <= -(Screen.width/2)
            || pos.y - this.GetComponent<RectTransform>().sizeDelta.y / 2 <= -(Screen.height / 2))
        {
            this.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            pos.x -= offset.x;
            pos.y -= offset.y;
        }
        else
        {
            this.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
            pos.x += offset.x;
            pos.y += offset.y;
        }

        DisplayInfo(inventory, id);
        this.GetComponent<RectTransform>().localPosition = pos;
    }

    private void DisplayInfo(InventoryObject inventory, int id)
    {
        sb = new StringBuilder();
        ItemObject item = inventory.Database.ItemsObjects[id];
        sb.Append("<size=35>").Append(item.name).Append("</size>").AppendLine();
        sb.Append(item.type.ItemTypeToString()).AppendLine().AppendLine();

        if(item.type == ItemType.Trinket)
        {
            TrinketObject trinket = (TrinketObject)item;
            AddStat(trinket.flatPowerBonus, "Power");
            AddStat(trinket.flatSpeedBonus, "Speed");
            AddStat(trinket.flatHealthBonus, "Health");
            AddStat(trinket.flatLuckBonus, "Luck");
            AddStat(trinket.flatDefenceBonus, "Defence");
            AddStat(trinket.flatCooldownReductionBonus, "Cooldown Reduction");
            AddStat(trinket.percentPowerBonus, "Power", true);
            AddStat(trinket.percentSpeedBonus, "Speed", true);
            AddStat(trinket.percentHealthBonus, "Health", true);
            AddStat(trinket.percentLuckBonus, "Luck", true);
            AddStat(trinket.percentDefenceBonus, "Defence", true);
            AddStat(trinket.percentCooldownReductionBonus, "Cooldown Reduction", true);
            sb.AppendLine();
        }
        sb.Append(item.description).AppendLine();
        infoText.text = sb.ToString();
        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }
    private void AddStat(float value, string statName, bool isPercent = false)
    {
        if(value != 0)
        {
            if (value > 0) sb.Append("+");

            if (isPercent) sb.Append(value * 100).Append("% ");
            else sb.Append(value).Append(" ");

            sb.Append(statName).AppendLine();

        }
    }

    public void Disable(GameObject obj, InventoryObject inventory, int id)
    {
        gameObject.SetActive(false);
    }

    public void OnInventoryDisable()
    {
        gameObject.SetActive(false);
    }
}
