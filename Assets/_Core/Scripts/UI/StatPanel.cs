using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPanel : MonoBehaviour
{
    public StatDisplay[] stats;

    private void OnValidate()
    {
        stats = gameObject.GetComponentsInChildren<StatDisplay>();
    }

    private void OnEnable()
    {
        Character.UpdateStatPanel += UpdateStatsValue;
    }

    private void OnDisable()
    {
        Character.UpdateStatPanel -= UpdateStatsValue;
    }

    public void UpdateStatsValue(params ModifiableStat[] playerStats)
    {
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].StatValue.text = playerStats[i].Value.ToString();
        }
    }
}
