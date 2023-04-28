using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// note(zack): for new input system
using UnityEngine.InputSystem;

public class PlayerTankBrain : MonoBehaviour, ITankBrain
{
    public AudioSource DriveAudio;

    private Vector2 _drive;
    private Vector2 _turret;
    private float _boost;
    private float _turbo;
    private float _fire;
    private bool _zoom;
    private bool _airbrake;
    private bool _switchMode;

    void OnSwitchMode(InputValue value) {
        _switchMode = value.Get<float>() > 0.1f;
    }

    void OnDrive(InputValue value) {
        _drive = value.Get<Vector2>();
        if (DriveAudio != null) {
            DriveAudio.volume = _drive.y * 0.2f;
            DriveAudio.Play();
        }
    }

    void OnTurnTurret(InputValue value) {
        if (Cursor.lockState == CursorLockMode.Locked) {
            var v = value.Get<Vector2>();
            _turret = new Vector2(-v.x, v.y) * 
                (Globals.Instance?.MouseSensitivity ?? 1f);
        } else {
            _turret.x = 0.0f;
            _turret.y = 0.0f;
        }
    }

    void OnZoom(InputValue value) {
        _zoom = value.Get<float>() > 0.0f;
    }

    void OnBoost(InputValue value) {
        _boost = value.Get<float>();
    }

    void OnTurbo(InputValue value) {
        _turbo = value.Get<float>();
    }

    void OnFire(InputValue value) {
        _fire = value.Get<float>();
    }

    void OnAirbrake(InputValue value) {
        _airbrake = value.Get<float>() > 0.5f;
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

    public float GetTurbo() {
        return _turbo;
    }

    public bool WantToZoom() {
        return _zoom;
    }
    
    public bool WantToFire() {
        return _fire > 0.0f;
    }

    public bool GetAirbrake() {
        return _airbrake;
    }

    public bool WantToSwitchMode() {
        var result = _switchMode;
        _switchMode = false;
        return result;
    }
}
