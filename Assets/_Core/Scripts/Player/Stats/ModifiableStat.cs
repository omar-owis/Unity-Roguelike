using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

[Serializable]
public class ModifiableStat
{
    public float BaseValue;
    public event Action OnChangeCallback;

    public virtual float Value
    {
        get
        {
            if(isDirty || BaseValue != lastBaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalcFinalMods();
                isDirty = false;
            }
            return _value;
        }
    }

    protected bool isDirty = true;
    protected float _value;
    protected float lastBaseValue = float.MinValue;

    protected readonly List<StatModifier> modifiers;
    public readonly ReadOnlyCollection<StatModifier> Modifiers;

    public ModifiableStat()
    {
        BaseValue = 0;
        modifiers = new List<StatModifier>();
        Modifiers = modifiers.AsReadOnly();
    }

    public ModifiableStat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        modifiers.Sort(CompareModifierOrder);
        isDirty = true;
        OnChangeCallback.Invoke();
    }

    public static int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order) return -1;
        else if (a.Order > b.Order) return 1;
        return 0;
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {

        if(modifiers.Remove(mod))
        {
            isDirty = true;
            OnChangeCallback.Invoke();
            return true;
        }
        return false;
    }

    public virtual void RemoveAllModifiers()
    {
        modifiers.Clear();
        isDirty = true;
        OnChangeCallback.Invoke();
    }

    public virtual bool RemoveModifierFromSource(object source)
    {
        bool didRemove = false;

        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            if (modifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                modifiers.RemoveAt(i);
            }
        }
        if(didRemove)
        {
            _value = CalcFinalMods();

            OnChangeCallback.Invoke();
        }

        return didRemove;
    }

    protected virtual float CalcFinalMods()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for(int i = 0; i < modifiers.Count; i++)
        {
            StatModifier mod = modifiers[i];

            if(mod.ModType == StatModType.Flat)
            {
                finalValue += mod.Value;
            }

            else if(mod.ModType == StatModType.PercentAdd)
            {
                sumPercentAdd += mod.Value;

                if(i + 1 > modifiers.Count || modifiers[i + 1].ModType != StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }

            else if(mod.ModType == StatModType.PercentMult)
            {
                finalValue *= 1 + mod.Value;
            }
        }

        return (float)Math.Round(finalValue, 4);
    }
}
