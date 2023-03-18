using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private float DetectedDistance;

    public GameObject Reticle;

    public float GetDistance() {
        return DetectedDistance;
    }

    void FixedUpdate()
    {
        int layerMask = 1 << 6 | 1 << 7;
        layerMask = ~layerMask;

        Vector3 rayDirection = transform.forward;
        Vector3 rayOrigin = transform.position;

        RaycastHit hit;

        var rayColor = Color.yellow;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, layerMask)) {
            DetectedDistance = hit.distance;
            Debug.DrawRay(rayOrigin, rayDirection * hit.distance, rayColor);
        } else {
            DetectedDistance = 1000.0f;
            Debug.DrawRay(rayOrigin, rayDirection * 1000.0f, rayColor);
        }

        if (Reticle) {
            Reticle.transform.position = rayOrigin + rayDirection * Mathf.Max(DetectedDistance - 3.0f, 0.0f);
            Reticle.transform.rotation = Quaternion.LookRotation(rayDirection);
        }
    }
}
