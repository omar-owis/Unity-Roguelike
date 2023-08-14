using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    public TextMeshProUGUI StatName;
    public TextMeshProUGUI StatValue;
    public Image StatImage;

    private void OnValidate()
    {
        StatValue = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        StatName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        StatImage = transform.GetChild(2).GetComponent<Image>();
    }
}
