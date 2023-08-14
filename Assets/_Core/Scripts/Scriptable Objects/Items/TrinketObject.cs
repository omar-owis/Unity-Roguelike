using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PercentType
{
    Additive,
    Multiplicative
}
[CreateAssetMenu(fileName = "New Trinket Object", menuName = "Inventory System/Items/Trinket")]
public class TrinketObject : ItemObject
{
    public float flatPowerBonus;
    public float flatSpeedBonus;
    public float flatHealthBonus;
    public float flatLuckBonus;
    public float flatCooldownReductionBonus;
    public float flatDefenceBonus;

    public float percentPowerBonus;
    public float percentSpeedBonus;
    public float percentHealthBonus;
    public float percentLuckBonus;
    public float percentCooldownReductionBonus;
    public float percentDefenceBonus;

    public PercentType percentBonusType;

    private void Awake()
    {
        type = ItemType.Trinket;
    }

    public override void EnableEffects(CharacterData c)
    {
        if (flatPowerBonus != 0)
            c.Power.AddModifier(new StatModifier(flatPowerBonus, StatModType.Flat, this));
        if (flatSpeedBonus != 0)
            c.Speed.AddModifier(new StatModifier(flatSpeedBonus, StatModType.Flat, this));
        //if (flatHealthBonus != 0)
        //    c.Health.AddModifier(new StatModifier(flatHealthBonus, StatModType.Flat, this));
        if (flatLuckBonus != 0)
            c.Luck.AddModifier(new StatModifier(flatLuckBonus, StatModType.Flat, this));
        if (flatCooldownReductionBonus != 0)
            c.CooldownReduction.AddModifier(new StatModifier(flatCooldownReductionBonus, StatModType.Flat, this));
        if (flatDefenceBonus != 0)
            c.Defence.AddModifier(new StatModifier(flatDefenceBonus, StatModType.Flat, this));


        if(percentBonusType == PercentType.Additive)
        {
            if (percentPowerBonus != 0)
                c.Power.AddModifier(new StatModifier(percentPowerBonus, StatModType.PercentAdd, this));
            if (percentSpeedBonus != 0)
                c.Speed.AddModifier(new StatModifier(percentSpeedBonus, StatModType.PercentAdd, this));
            //if (percentHealthBonus != 0)
            //    c.Health.AddModifier(new StatModifier(percentHealthBonus, StatModType.PercentAdd, this));
            if (percentLuckBonus != 0)
                c.Luck.AddModifier(new StatModifier(percentLuckBonus, StatModType.PercentAdd, this));
            if (percentCooldownReductionBonus != 0)
                c.CooldownReduction.AddModifier(new StatModifier(percentCooldownReductionBonus, StatModType.PercentAdd, this));
            if (percentDefenceBonus != 0)
                c.Defence.AddModifier(new StatModifier(percentDefenceBonus, StatModType.PercentAdd, this));
        }
        else
        {
            if (percentPowerBonus != 0)
                c.Power.AddModifier(new StatModifier(percentPowerBonus, StatModType.PercentMult, this));
            if (percentSpeedBonus != 0)
                c.Speed.AddModifier(new StatModifier(percentSpeedBonus, StatModType.PercentMult, this));
            //if (percentHealthBonus != 0)
            //    c.Health.AddModifier(new StatModifier(percentHealthBonus, StatModType.PercentMult, this));
            if (percentLuckBonus != 0)
                c.Luck.AddModifier(new StatModifier(percentLuckBonus, StatModType.PercentMult, this));
            if(percentCooldownReductionBonus != 0)
                c.CooldownReduction.AddModifier(new StatModifier(percentCooldownReductionBonus, StatModType.PercentMult, this));
            if (percentDefenceBonus != 0)
                c.Defence.AddModifier(new StatModifier(percentDefenceBonus, StatModType.PercentMult, this));
        }
    }
    public override void UnenableEffects(CharacterData c)
    {
        c.Power.RemoveModifierFromSource(this);
        c.Speed.RemoveModifierFromSource(this);
        //c.Health.RemoveModifierFromSource(this);
        c.Luck.RemoveModifierFromSource(this);
        c.CooldownReduction.RemoveModifierFromSource(this);
    }
}


