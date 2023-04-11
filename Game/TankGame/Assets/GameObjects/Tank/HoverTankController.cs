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
    private Vector2     _tankPitchYaw = new Vector2(
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

    // Note(Zack): Called by Respawn Manager to know when to respawn the tank.
    public bool IsAlive() {
        return  TankBody != null && 
                TankHead != null && 
                TankTurret != null;
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > 2) {
            SoundManager.Instance.PlaySound(SoundAsset.Collision);
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
        _tankPitchYaw.y = 0.0f;
        var forwardDir = -TankBody.transform.forward;
        _tankPitchYaw.x = Mathf.Atan2(forwardDir.z, forwardDir.x); 
    }

    void Update()
    {
        if (IsAlive() == false) { 
            _tankRigidbody.centerOfMass = new Vector3(0,0,0);
            return; 
        }
        _boostTimer -= Time.deltaTime;

        Vector2 TurretInput = _brain.GetTurretInput() * Time.deltaTime * 0.1f;
        Vector2 DriveInput = _brain.GetDriveInput() * Time.deltaTime * 100.0f;
        float BoostDir = _brain.GetBoost();
        bool Airbrake = _brain.GetAirbrake();

        if (_mode == TankMode.COMBAT) {
            _tankPitchYaw += TurretInput;
        } else if (_mode == TankMode.DRIVE) {
            _setTurretForward();
        }
        _tankRigidbody.drag = Airbrake ? 0.99f : _startDrag;
        
        Vector3 toTarget = _turretDirection(_tankPitchYaw);
        // toTarget = TankBody.transform.TransformDirection(toTarget);

        Debug.DrawRay(TankHead.transform.position, toTarget * 20.0f, Color.green);
        float stepSize = 0.5f * Time.deltaTime;

        Vector3 nextRotation = Vector3.RotateTowards(TankHead.transform.forward, -toTarget, stepSize, 0.0f);

        if (nextRotation.magnitude > 0.0f) {
            TankHead.transform.rotation = Quaternion.LookRotation(nextRotation, TankBody.transform.up);
        } else {
            Debug.Log($"{name}: has weird rotation, turret = {_tankPitchYaw}, {toTarget}");
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
