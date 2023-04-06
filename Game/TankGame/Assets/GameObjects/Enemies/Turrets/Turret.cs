using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject   TurretBody;
    public GameObject   TurretBarrel;
    [Range(0, 1)]
    public float        TurnSpeed;

    [Range(0, 1)]
    public float AimTolerance;

    private TargetFinder _targetFinder;
    private IWeapon     _weapon;

    private float _variance = 4.0f;

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
        Vector3 targetDirection = Vector3.Normalize(_targetFinder.Target - TurretBarrel.transform.position);
        float accuracy = Vector3.Dot(cannonAim, targetDirection);
        bool result = accuracy > AimTolerance;

        Debug.DrawRay(TurretBarrel.transform.position, cannonAim, result ? Color.red : Color.yellow); 

        return result;
    }

    void FixedUpdate()
    {
        Vector3 variance = new Vector3(
            Random.Range(-_variance, _variance),
            Random.Range(-_variance, _variance),
            Random.Range(-_variance, _variance)
        );
        Vector3 toTarget = Vector3.Normalize(_targetFinder.Target - TurretBarrel.transform.position);
        float stepSize = TurnSpeed * Time.fixedDeltaTime;

        Vector3 nextRotation = Vector3.RotateTowards(TurretBarrel.transform.forward, toTarget, stepSize, 0.0f);

        TurretBarrel.transform.rotation = Quaternion.LookRotation(nextRotation);

        if (_wantToFire() && _weapon != null) {
            _weapon.Fire();
        }
    }
}
