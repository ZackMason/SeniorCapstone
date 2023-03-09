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
        return new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );
    }

    public Vector2 GetTurretInput() {
        return _turret;
        return new Vector2(
            Input.GetAxis("Mouse X") + Input.GetAxis("Right Stick Horizontal") * 0.1f,
            Input.GetAxis("Mouse Y") + Input.GetAxis("Right Stick Vertical") * 0.1f
        );
    }

    public float GetBoost() {
        return _boost;
        return Input.GetAxis("Boost");
    }

    public bool WantToZoom() {
        return _zoom;
        return Input.GetAxis("Fire2") > 0.0f;
    }
    
    public bool WantToFire() {
        return _fire > 0.0f;
    }
}
