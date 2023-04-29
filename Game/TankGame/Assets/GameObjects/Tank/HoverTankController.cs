using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TankMode {
    DRIVE, COMBAT, SIZE
};

public class HoverTankController : MonoBehaviour
{
    public ITankBrain   _brain;
    private HEWeapon    _weapon;
    private Rigidbody   _tankRigidbody;
    private Camera      _camera;
    private Vector2     _tankYawPitch = new Vector2(
        0.0f, Mathf.PI
    );

    [SerializeField]
    private TankMode _mode = TankMode.DRIVE;

    [Range(0, 15)]
    public float BoostCooldownTime;
    [Range(0, 15)]
    public float TurboCooldownTime;
    private float _boostTimer;
    private float _turboTimer;

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

    [SerializeField] private PIDController _torqueXController;
    [SerializeField] private PIDController _torqueZController;

    public void OverrideDeathTimer() => _deathTimer = 10000.0f;

    void Start() {
        Debug.Assert(TankBody != null);
        Debug.Assert(TankTurret != null);
        Debug.Assert(TankHead != null);

        if (_brain == null) {
            _brain = GetComponent<ITankBrain>();
        }
        _weapon = GetComponentInChildren<HEWeapon>();
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
            SoundManager.Instance?.PlaySound(SoundAsset.Collision,TankHead.transform.position);
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

    void Update() {
        if (IsAlive() == false) { 
            _tankRigidbody.centerOfMass = new Vector3(0,0,0);
            if ((_deathTimer -= Time.deltaTime) < 0.0f) {
                Destroy(gameObject);
            }
            return; 
        }
        _boostTimer -= Time.deltaTime;
        _turboTimer -= Time.deltaTime;

        Vector2 TurretInput = _brain.GetTurretInput() * Time.deltaTime * 0.1f;
        if (_mode == TankMode.COMBAT) {
            _tankYawPitch += TurretInput;
            float maxPitch = 20f * Mathf.PI / 180f;
            if (_tankYawPitch.y > 180f) {
                _tankYawPitch.y -= 360f;
            }
            _tankYawPitch.y = Mathf.Clamp(_tankYawPitch.y, -maxPitch, maxPitch);
        } else if (_mode == TankMode.DRIVE) {
            _setTurretForward();
        }
        Vector3 toTarget = _turretDirection(_tankYawPitch);
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

        if (_camera != null) {
            if (_brain.WantToZoom()) {
                _camera.fieldOfView = 30;
            } else {
                _camera.fieldOfView = 90;
            }
        }

        if (_brain.WantToFire() && _weapon != null) {
            if (_weapon.Fire()) {
                _tankRigidbody.AddForceAtPosition(_weapon.transform.forward * 100f, _weapon.transform.position, ForceMode.Impulse);
            }
        }

        if (_brain.WantToSwitchMode()) {
            _mode = _mode == TankMode.COMBAT ? TankMode.DRIVE : TankMode.COMBAT;
        }
    }

    void FixedUpdate() {
        if (IsAlive() == false) { 
            return;
        }
        Vector2 DriveInput = _brain.GetDriveInput() * Time.fixedDeltaTime * 100.0f;

        if (DriveInput == Vector2.zero && 
            _tankRigidbody.velocity.magnitude < 0.1f) {
            _tankRigidbody.velocity = Vector3.zero;
        }

        float BoostDir = _brain.GetBoost();
        float TurboActive = _brain.GetTurbo();
        bool Airbrake = _brain.GetAirbrake();

        // var tankRotation = Vector3.Dot(TankBody.transform.up, Vector3.up);
        // var pidXTorque = _torqueXController.Calculate(Time.fixedDeltaTime, tankRotation, 0f);
        // var pidZTorque = _torqueZController.Calculate(Time.fixedDeltaTime, tankRotation, 0f);
        // _tankRigidbody.AddTorque(-Vector3.right * TorquePower * pidXTorque);
        // _tankRigidbody.AddTorque(Vector3.forward * TorquePower * pidZTorque);


        _tankRigidbody.drag = Airbrake ? 2.99f : _startDrag;
        
        Vector3 TurretForward = -TankHead.transform.forward;
        Vector3 BodyForward = -TankBody.transform.forward;
        Vector3 BodyRight = -TankBody.transform.right;
        TurretForward.y = 0.0f;
        BodyRight.y = 0.0f;

        _tankRigidbody.AddTorque(Vector3.up * DriveInput.x * TorquePower);
        _tankRigidbody.AddForce(BodyForward * DriveInput.y * DrivePower);

        if (TurboActive != 0.0f && _turboTimer <= 0.0f){
            _turboTimer = TurboCooldownTime;
            _boostTimer = BoostCooldownTime * 0.5f;
            //Debug.Log("turbo");
            _tankRigidbody.AddForce(BodyForward * TurboActive * DrivePower * .5f, ForceMode.Impulse);
        }

        else if (BoostDir != 0.0f && _boostTimer <= 0.0f) {
            _boostTimer = BoostCooldownTime;
            //Debug.Log("boost");
            _tankRigidbody.AddForce(BodyRight * BoostDir * DrivePower * 3.0f, ForceMode.Impulse);
        }
        

    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + CenterOfMass, 0.2f);
    }
}
