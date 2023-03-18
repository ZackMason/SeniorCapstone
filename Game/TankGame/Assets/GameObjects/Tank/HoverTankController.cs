using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTankController : MonoBehaviour
{
    public ITankBrain  _brain;
    private IWeapon     _weapon;
    private Rigidbody   _tankRigidbody;
    private Camera      _camera;

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

    [Range(0, 200)]
    public float TorquePower;

    private float _startDrag;

    void Awake()
    {
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
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > 2) {
            SoundManager.Instance.PlaySound(SoundAsset.Collision);
        }
            // saudio.Play();

    }
    void FixedUpdate()
    {

        
        Vector2 currentTurretInput = new Vector2(TankTurret.transform.rotation.x, TankHead.transform.rotation.y);
        
        _boostTimer -= Time.fixedDeltaTime;

        Vector2 TurretInput = _brain.GetTurretInput() * Time.fixedDeltaTime * 50.0f;
        Vector2 DriveInput = _brain.GetDriveInput() * Time.fixedDeltaTime * 100.0f;
        float BoostDir = _brain.GetBoost();
        bool Airbrake = _brain.GetAirbrake();

        _tankRigidbody.drag = Airbrake ? 0.99f : _startDrag;

        currentTurretInput = Vector2.Lerp(currentTurretInput, TurretInput, LerpConstant * Time.deltaTime);

        if (Cursor.lockState == CursorLockMode.Locked) {
            TankTurret.transform.Rotate(currentTurretInput.y, 0.0f, 0.0f, Space.Self);
            TankHead.transform.Rotate(0.0f, currentTurretInput.x, 0.0f, Space.Self);
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
    }
}
