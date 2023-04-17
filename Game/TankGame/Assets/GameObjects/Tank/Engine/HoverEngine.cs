using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoverEngine : MonoBehaviour
{
    public Rigidbody TankRigidbody;

    [Range(1, 6000)]
    public float EnginePower;
    
    private float _HoverPower(float groundDistance) {
        return EnginePower / Mathf.Clamp(groundDistance, 0.1f, 10.0f);
    }

    void Start() {
        if (TankRigidbody == null) {
            TankRigidbody = GetComponentInParent<Rigidbody>();
        }
        Debug.Assert(TankRigidbody != null);
    }

    void FixedUpdate() {
        int layerMask = 1 << 6 | 1 << 7;
        layerMask = ~layerMask;

        RaycastHit hit;

        Vector3 rayDirection = transform.TransformDirection(Vector3.down);
        Vector3 rayOrigin = transform.position;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, layerMask)) {
            Vector3 halfVector = Vector3.Normalize(-rayDirection + hit.normal);

            float cosFactor = Vector3.Dot(-rayDirection, hit.normal);
            // cosFactor = 1.0f;
            // halfVector = -rayDirection;
  
            TankRigidbody.AddForceAtPosition(_HoverPower(hit.distance) * halfVector * cosFactor * Time.fixedDeltaTime * 10.0f, rayOrigin);

            Debug.DrawRay(rayOrigin, halfVector * _HoverPower(hit.distance) * cosFactor * 0.1f, Color.blue);
            Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.yellow);
        } else {
            Debug.DrawRay(rayOrigin, rayDirection * 100.0f, Color.yellow);
        }
    }
}
