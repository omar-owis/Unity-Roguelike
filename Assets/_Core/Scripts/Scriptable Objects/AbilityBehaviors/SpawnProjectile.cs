using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Pool;
using System.Collections.Generic;
using UnityEditor;

public class SpawnProjectile : AbilityBehavior
{
    public Hand hand;
    public GameObject _projectile;
    public float _damage;
    public float _speed;
    private Transform _shootPoint;
    private AbilityController _controller;

    //private ObjectPool<GameObject> _pool;
    private LayerMask _whatIsPlayer;
    public override Task Execute(float effectivenessFactor)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        // if raycast hit a collider
        if (Physics.Raycast(ray, out hit, ~_whatIsPlayer))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 shootDirection = targetPoint - _shootPoint.position;

        GameObject projectileObject = _controller.SpawnObject(_projectile, _shootPoint.position);
        projectileObject.transform.forward = shootDirection;

        return Task.CompletedTask;
    }

    public override void Initialize(AbilityController abilityController)
    {
        _shootPoint = abilityController.GetShootPoint(hand);
        _whatIsPlayer = LayerMask.NameToLayer("WhatIsPlayer");
        _controller = abilityController;
    }
}
