using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// note(zack): for new input system
using UnityEngine.InputSystem;

public class PlayerTankBrain : MonoBehaviour, ITankBrain
{
    private Vector2 _drive;
    private Vector2 _turret;
    private float _boost;
    private bool _zoom;
    private float _fire;

    void OnDrive(InputValue value) {
        _drive = value.Get<Vector2>();
    }

    void OnTurnTurret(InputValue value) {
        _turret = value.Get<Vector2>();
    }

    void OnZoom(InputValue value) {
        _zoom = value.Get<float>() > 0.0f;
    }

    void OnBoost(InputValue value) {
        _boost = value.Get<float>();
    }

    void OnFire(InputValue value) {
        _fire = value.Get<float>();
    }

    public Vector2 GetDriveInput() {
        return _drive;
    }

    public Vector2 GetTurretInput() {
        return _turret;
    }

    public float GetBoost() {
        return _boost;
    }

    public bool WantToZoom() {
        return _zoom;
    }
    
    public bool WantToFire() {
        return _fire > 0.0f;
    }
}
