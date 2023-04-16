using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HammerSpin : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private Transform _centerPosition;

    void Start() {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerPosition.position - transform.position;
    }

    void FixedUpdate()
    {
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(new Vector3(0f, 500f * Time.fixedDeltaTime, 0f)));
    }
}
