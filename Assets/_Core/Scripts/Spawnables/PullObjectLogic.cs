using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObjectLogic : SpawnableObject
{
    private BoxCollider _collider;
    public Vector3 Size { get { return _collider.size; } set { _collider.size = value; } }
    public float PullForce;
    // Add VFX parameter

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody != null && other.tag != "Player")
        {
            Vector3 pullDirection = transform.position - other.gameObject.transform.position;
            other.attachedRigidbody.AddForce(pullDirection.normalized * PullForce);
        }
    }
}
