using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TankMode {
    DRIVE, COMBAT, SIZE
};

public class HoverTankController : MonoBehaviour
{
    public ITankBrain  _brain;
    private IWeapon     _weapon;
    private Rigidbody   _tankRigidbody;
    private Camera      _camera;
    private Vector2     _tankYawPitch = new Vector2(
        0.0f, Mathf.PI
    );

    private TankMode _mode = TankMode.COMBAT;

    [Range(0, 15)]
    public float BoostCooldownTime;
    private float _boostTimer;

    public Vector3      CenterOfMass;
    public GameObject   TankHead;
    public GameObject   TankTurret;
    public GameObject   TankBody;
    public float        LerpConstant;

    [Range(1, 2000)]
    public float DrivePower;

    [Range(0, 800)]
    public float TorquePower;

    private float _startDrag;
    private float _deathTimer = 5.0f;

    public void OverrideDeathTimer() => _deathTimer = 10000.0f;

    void Start() {
        Debug.Assert(TankBody != null);
        Debug.Assert(TankTurret != null);
        Debug.Assert(TankHead != null);

        if (_brain == null) {
            _brain = GetComponent<ITankBrain>();
        }
        _weapon = GetComponentInChildren<IWeapon>();
        _camera = GetComponentInChildren<Camera>();
        _tankRigidbody = GetComponent<Rigidbody>();

        _tankRigidbody.centerOfMass = CenterOfMass;

        _startDrag = _tankRigidbody.drag;

        Debug.Assert(_brain != null);
        Debug.Assert(_weapon != null);

        _setTurretForward();
        UnitManager.Instance.AddUnit(gameObject);
    }

    public void OnDestroy() {
        // Debug.Log($"{name} Destroyed");
        UnitManager.Instance.RemoveUnit(gameObject);
    }

    // Note(Zack): Called by Respawn Manager to know when to respawn the tank.
    public bool IsAlive() {
        return  TankBody != null && 
                TankHead != null && 
                TankTurret != null;
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > 2) {
            SoundManager.Instance?.PlaySound(SoundAsset.Collision);
        }
    }

    Vector3 _turretDirection(Vector2 yawPitch) {
        var yaw = yawPitch.x;
        var pitch = yawPitch.y;
        return new Vector3(
            Mathf.Cos((yaw)) * Mathf.Cos((pitch)),
            Mathf.Sin((pitch)),
            Mathf.Sin((yaw)) * Mathf.Cos((pitch))
        );
    }

    private void _setTurretForward() {
        _tankYawPitch.y = 0.0f;
        var forwardDir = -TankBody.transform.forward;
        _tankYawPitch.x = Mathf.Atan2(forwardDir.z, forwardDir.x); 
    }

    void Update()
    {
        if (IsAlive() == false) { 
            _tankRigidbody.centerOfMass = new Vector3(0,0,0);
            if ((_deathTimer -= Time.deltaTime) < 0.0f) {
                Destroy(gameObject);
            }
            return; 
        }
        _boostTimer -= Time.deltaTime;

        Vector2 TurretInput = _brain.GetTurretInput() * Time.deltaTime * 0.1f;
        Vector2 DriveInput = _brain.GetDriveInput() * Time.deltaTime * 100.0f;
        float BoostDir = _brain.GetBoost();
        bool Airbrake = _brain.GetAirbrake();

        if (_mode == TankMode.COMBAT) {
            _tankYawPitch += TurretInput;
            float maxPitch = 12f * Mathf.PI / 180f;
            if (_tankYawPitch.y > 180f) {
                _tankYawPitch.y -= 360f;
            }
            _tankYawPitch.y = Mathf.Clamp(_tankYawPitch.y, -maxPitch, maxPitch);
        } else if (_mode == TankMode.DRIVE) {
            _setTurretForward();
        }
        _tankRigidbody.drag = Airbrake ? 0.99f : _startDrag;
        
        Vector3 toTarget = _turretDirection(_tankYawPitch);
        // toTarget = TankBody.transform.TransformDirection(toTarget);

        Debug.DrawRay(TankHead.transform.position, toTarget * 20.0f, Color.green);
        float stepSize = 0.5f * Time.deltaTime;

        Vector3 nextRotation = Vector3.RotateTowards(TankHead.transform.forward, -toTarget, stepSize, 0.0f);

        if (nextRotation.magnitude > 0.0f) {
            float maxPitchAngle = 12f;
            Quaternion newRotation = Quaternion.LookRotation(nextRotation, TankBody.transform.up);
            Vector3 euler = newRotation.eulerAngles;
            if (euler.x > 180f) {
                euler.x -= 360f;
            }
            euler.x = Mathf.Clamp(euler.x, -maxPitchAngle, maxPitchAngle);
            newRotation.eulerAngles = euler;
            TankHead.transform.rotation = newRotation;
        } else {
            Debug.Log($"{name}: has weird rotation, turret = {_tankYawPitch}, {toTarget}");
        }

        Vector3 TurretForward = -TankHead.transform.forward;
        Vector3 BodyForward = -TankBody.transform.forward;
        Vector3 BodyRight = -TankBody.transform.right;
        TurretForward.y = 0.0f;
        BodyRight.y = 0.0f;

        _tankRigidbody.AddTorque(Vector3.up * DriveInput.x * TorquePower);
        _tankRigidbody.AddForce(BodyForward * DriveInput.y * DrivePower);

        if (BoostDir != 0.0f && _boostTimer <= 0.0f) {
            _boostTimer = BoostCooldownTime;
            _tankRigidbody.AddForce(BodyRight * BoostDir * DrivePower * 4.0f, ForceMode.Impulse);
        }

        if (_camera != null) {
            if (_brain.WantToZoom()) {
                _camera.fieldOfView = 30;
            } else {
                _camera.fieldOfView = 90;
            }
        }
        if (_brain.WantToFire() && _weapon != null) {
            _weapon.Fire();
        }
        if (_brain.WantToSwitchMode()) {
            _mode = _mode == TankMode.COMBAT ? TankMode.DRIVE : TankMode.COMBAT;
        }
    }
}
