using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEngine : MonoBehaviour
{
    public Rigidbody TankRigidbody;

    [Range(1, 2000)]
    public float EnginePower;
    
    private float _HoverPower(float groundDistance) {
        return 1.0f / groundDistance;
    }

    void Start()
    {
        Debug.Assert(TankRigidbody != null);
    }

    void FixedUpdate()
    {
        int layerMask = 1 << 6 | 1 << 7;
        layerMask = ~layerMask;

        RaycastHit hit;

        Vector3 rayDirection = transform.TransformDirection(Vector3.down);
        Vector3 rayOrigin = transform.position;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, layerMask)) {
            Vector3 halfVector = Vector3.Normalize(-rayDirection + hit.normal);

            float cosFactor = Vector3.Dot(-rayDirection, hit.normal);
            // cosFactor = 1.0f;
  
            TankRigidbody.AddForceAtPosition(_HoverPower(hit.distance) * halfVector * cosFactor * EnginePower, rayOrigin);

            Debug.DrawRay(rayOrigin, halfVector * _HoverPower(hit.distance) * cosFactor, Color.blue);
            Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.yellow);
        } else {
            Debug.DrawRay(rayOrigin, rayDirection * 100.0f, Color.yellow);
        }
    }
}
