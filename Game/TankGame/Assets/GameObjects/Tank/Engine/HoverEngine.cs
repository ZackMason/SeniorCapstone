using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class PIDController {
    public enum DerivativeMeasurement {
        Velocity, ErrorRateOfChange
    }

    public float proportionGain = .8f;
    public float derivativeGain = 0.3f;
    public float integralGain = 0.7f;
    public float integralSaturation = 0f;

    public float outputMin = -1f;
    public float outputMax = 1f;

    [Header("Don't Touch")]

    public float integrationStored = 0f;

    public float errorLast = 0f;
    public float valueLast = 0f;
    public DerivativeMeasurement derivativeMeasurement = DerivativeMeasurement.ErrorRateOfChange;

    public bool derivativeInitialized = false;
    public void Reset() { 
        derivativeInitialized = false;
        integrationStored = 0f;
    }

    public float Calculate(float dt, float currentValue, float targetValue) {
        float error = targetValue - currentValue;

        // Proportional
        float P = proportionGain * error;

        // Integral
        integrationStored = Mathf.Clamp(integrationStored + (error * dt),
            -integralSaturation, integralSaturation);
        float I = integralGain * integrationStored;

        // Derivative
        float errorRateOfChange = (error - errorLast) / dt;
        errorLast = error;

        float valueRateOfChange = (currentValue - valueLast) / dt;
        valueLast = currentValue;

        float D = 0f;
        if (derivativeInitialized) {
            D = derivativeGain * 
                (derivativeMeasurement == DerivativeMeasurement.Velocity ?
                -valueRateOfChange : errorRateOfChange);
        } else {
            derivativeInitialized = true;
        }
 
        return Mathf.Clamp(P + I + D, outputMin, outputMax);
    }    
}

public class HoverEngine : MonoBehaviour
{
    [SerializeField] private bool _showInfo = false;
    [SerializeField] private PIDController _pidRotController;
    [SerializeField] private PIDController _pidDistanceController;
    public Rigidbody TankRigidbody;

    [Range(1, 60000)]
    public float EnginePower;

    private float _lastDistance = 0f;
    private float _lastPower = 0f;
    
    private float _HoverPower(float groundDistance) {
        groundDistance = Mathf.Clamp(groundDistance, 0f, 10f);
        _lastDistance = groundDistance;
        
        float rotError = Vector3.Dot(Vector3.up, TankRigidbody.transform.up);
        float pidRotPower = _pidRotController.Calculate(Time.fixedDeltaTime, rotError, 1f);
        float pidDistPower = _pidDistanceController.Calculate(Time.fixedDeltaTime, groundDistance, 1.2f + Mathf.PerlinNoise(1f, Time.time)*.2f-.1f);
                
        return EnginePower * pidDistPower + EnginePower * pidRotPower * 0.1f;
        // return (EnginePower * 0.1f) / Mathf.Max(0.1f, groundDistance);
    }

    void Start() {
        _pidRotController.Reset();
        _pidDistanceController.Reset();
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
            _lastPower = _HoverPower(hit.distance);
            TankRigidbody.AddForceAtPosition(_lastPower * halfVector * cosFactor * Time.fixedDeltaTime * 10.0f, rayOrigin);

            Debug.DrawRay(rayOrigin, halfVector * _HoverPower(hit.distance) * cosFactor * 0.1f, Color.blue);
            Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.yellow);
        } else {
            Debug.DrawRay(rayOrigin, rayDirection * 100.0f, Color.yellow);
        }
    }

    
    // void OnDrawGizmos() {
    //     if (_showInfo) {
    //         var errorLast = _pidDistanceController.errorLast;
            
    //         Color labelColor = Color.black;
    //         GUI.contentColor = labelColor;

    //         GUIStyle style = new GUIStyle(GUI.skin.label);

    //         style.fontSize = 20;

    //         Handles.Label(transform.position + transform.right, $"Hover Engine\nError: {errorLast}\nDistance: {_lastDistance}\nPower: {_lastPower}", style);
    //     }
    // }
}
