using DungeonMan.CustomAttributes;
using System.Threading.Tasks;
using UnityEngine;

public class LinearMove : AbilityBehavior
{
    private AbilityController _controller;
    [StaticBool("_directionBool")] public Vector3 _direction;
    [StaticBool("_speedBool")] public float _speed;
    [StaticBool("_durationBool")] public int _duration;

    [HideInInspector] public bool _directionBool;
    [HideInInspector] public bool _speedBool;
    [HideInInspector] public bool _durationBool;

    public override async Task Execute(float effectivenessFactor)
    {
        if (_directionBool) _controller.MovementControl = _direction * effectivenessFactor;
        else _controller.MovementControl = _direction;

        if (_speedBool) _controller.MoveSpeed = _speed * effectivenessFactor;
        else _controller.MoveSpeed = _speed;

        if (_durationBool) await Task.Delay((int)(_duration * effectivenessFactor));
        else await Task.Delay(_duration);

        _controller.MovementControl = Vector3.zero;
    }

    public override void Initialize(AbilityController abilityController)
    {
        _controller = abilityController;
    }

    private void OnValidate()
    {
        _duration = Mathf.Clamp(_duration, 0, int.MaxValue);
    }
}