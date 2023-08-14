using DungeonMan.CustomAttributes;
using System.Threading.Tasks;
using UnityEngine;

public class StatDuration : AbilityBehavior
{
    private AbilityController _controller;
    [StaticBool("_durationBool")] public int _duration;
    [StaticBool("_valueBool")] public float _appliedValue;
    public StatType _type;
    public StatModType _statModType = StatModType.Flat;

    [HideInInspector] public bool _valueBool;
    [HideInInspector] public bool _durationBool;

    public override async Task Execute(float effectivenessFactor)
    {
        float appliedValue;
        if (_valueBool) appliedValue = _appliedValue * effectivenessFactor;
        else appliedValue = _appliedValue;

        switch (_type)
        {
            case StatType.MaxHealth:
                _controller.CharachterData.maxHealth.AddModifier(new StatModifier(appliedValue, _statModType, this));
                break;
            case StatType.Speed:
                _controller.CharachterData.Speed.AddModifier(new StatModifier(appliedValue, _statModType, this));
                break;
            case StatType.Luck:
                _controller.CharachterData.Luck.AddModifier(new StatModifier(appliedValue, _statModType, this));
                break;
            case StatType.Power:
                _controller.CharachterData.Power.AddModifier(new StatModifier(appliedValue, _statModType, this));
                break;
            case StatType.CooldownReduction:
                _controller.CharachterData.CooldownReduction.AddModifier(new StatModifier(appliedValue, _statModType, this));
                break;
            case StatType.Defence:
                _controller.CharachterData.Defence.AddModifier(new StatModifier(appliedValue, _statModType, this));
                break;
            default:
                break;
        }

        if (_durationBool) await Task.Delay((int)(_duration * effectivenessFactor));
        else await Task.Delay(_duration);

        switch (_type)
        {
            case StatType.MaxHealth:
                _controller.CharachterData.maxHealth.RemoveModifierFromSource(this);
                break;
            case StatType.Speed:
                _controller.CharachterData.Speed.RemoveModifierFromSource(this);
                break;
            case StatType.Luck:
                _controller.CharachterData.Luck.RemoveModifierFromSource(this);
                break;
            case StatType.Power:
                _controller.CharachterData.Power.RemoveModifierFromSource(this);
                break;
            case StatType.CooldownReduction:
                _controller.CharachterData.CooldownReduction.RemoveModifierFromSource(this);
                break;
            case StatType.Defence:
                _controller.CharachterData.Defence.RemoveModifierFromSource(this);
                break;
            default:
                break;
        }
    }

    public override void Initialize(AbilityController abilityController)
    {
        _controller = abilityController;
    }
}
