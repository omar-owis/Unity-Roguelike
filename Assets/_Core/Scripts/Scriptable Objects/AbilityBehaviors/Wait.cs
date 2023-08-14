using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using DungeonMan.CustomAttributes;

public class Wait : AbilityBehavior
{
    [StaticBool("_durationBool")] public int _duration;
    [HideInInspector] public bool _durationBool;

    public override async Task Execute(float effectivenessFactor)
    {
        if (_durationBool) await Task.Delay((int)(_duration * effectivenessFactor));
        else await Task.Delay(_duration);
    }

    public override void Initialize(AbilityController abilityController)
    {
        _waitUntil = true;
    }

    private void OnValidate()
    {
        _duration = Mathf.Clamp(_duration, 0, int.MaxValue);
        _waitUntil = true;
    }
}
