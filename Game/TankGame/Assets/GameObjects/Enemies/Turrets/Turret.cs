using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject   TurretBody;
    public GameObject   TurretBarrel;
    [Range(0, 100)]
    public float        TurnSpeed;

    [Range(0, 1)]
    public float AimAccuracy;

    private TargetFinder _targetFinder;
    private IWeapon     _weapon;

    void Awake()
    {
        Debug.Assert(TurretBody != null);
        Debug.Assert(TurretBarrel != null);

        if (_targetFinder == null) {
            _targetFinder = GetComponent<TargetFinder>();
        }
        _weapon = GetComponentInChildren<IWeapon>();

        Debug.Assert(_weapon != null);
    }

    private bool _wantToFire() {
        Vector3 cannonAim = TurretBarrel.transform.forward;
        Vector3 targetDirection = _targetFinder.Target - TurretBarrel.transform.position;
        float accuracy = Vector3.Dot(cannonAim, targetDirection);
        return accuracy > AimAccuracy;
    }


    void FixedUpdate()
    {
        Vector3 toTarget = Vector3.Normalize(_targetFinder.Target - TurretBarrel.transform.position);
        float stepSize = TurnSpeed * Time.fixedDeltaTime;

        Vector3 nextRotation = Vector3.RotateTowards(TurretBarrel.transform.forward, toTarget, stepSize, 0.0f);

        TurretBarrel.transform.rotation = Quaternion.LookRotation(nextRotation);

        // Vector2 currentTurretInput = new Vector2(TurretBarrel.transform.rotation.eulerAngles.x, TurretBody.transform.rotation.eulerAngles.y);
        // Vector2 TurretInput = _brain.GetTurretInput() * Time.fixedDeltaTime * 50.0f;
        // currentTurretInput = Vector2.Lerp(currentTurretInput, TurretInput, 50 * Time.deltaTime);

        // TurretBarrel.transform.Rotate(currentTurretInput.y, 0.0f, 0.0f, Space.Self);
        // TurretBody.transform.Rotate(0.0f, currentTurretInput.x, 0.0f, Space.Self);

        if (_wantToFire() && _weapon != null) {
            _weapon.Fire();
        }
    }
}
