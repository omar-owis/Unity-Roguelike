using System;
using UnityEngine;

[Serializable]
public enum Hand
{
    left,
    right
}
public abstract class AbstractAbilityObject : ScriptableObject
{
    public float cooldown;
    // add variable for sound
    // add variable for animation and animation controller
    public Sprite UIDisplay;
    [TextArea(15, 20)]
    public string description;
    protected bool _isChargable = false;

    public bool IsChargable { get { return _isChargable; } }

    public abstract void Init(AbilityController player);

    public abstract void Activate(float charge);
}
