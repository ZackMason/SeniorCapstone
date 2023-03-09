using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankBrain : MonoBehaviour, ITankBrain
{
<<<<<<< Updated upstream
    public Vector3 Target;
=======
    private TargetFinder TargetFinder;
    public Transform Base;
    public Transform Cannon;
>>>>>>> Stashed changes

    void Start() {
        TargetFinder = GetComponent<TargetFinder>(); 
    }

    public Vector2 GetDriveInput() {
        float fuzzy_forward = Vector3.Dot(-transform.forward, Vector3.Normalize(TargetFinder.Target - transform.position));
        float fuzzy_right = Vector3.Dot(-transform.right, Vector3.Normalize(TargetFinder.Target - transform.position));

        return new Vector2(
            Mathf.Clamp(fuzzy_right*2.0f, -1.0f, 1.0f),
            fuzzy_forward
        );
    }

    public float GetBoost() {
        return 0.0f;
    }

    public Vector2 GetTurretInput() {
<<<<<<< Updated upstream
        // TODO(ZACK): do turret
        return new Vector2(
            0.0f, 0.0f
        );
=======
        Vector3 projectedVector = Vector3.ProjectOnPlane(TargetFinder.Target - Base.position, -Base.up);
        float BaseTurn = Vector3.SignedAngle(projectedVector, Base.forward, Base.up);
        float BarrelAngle = 90-Vector3.Angle(TargetFinder.Target, Cannon.up);

        // Cannon.up = Vector3.up;
        // Vector3 right = Vector3.Cross(Cannon.forward, Vector3.up);
        // Cannon.rotation = Quaternion.LookRotation(right, Vector3.up);

        // Vector3 targetDirection = Target - transform.position;
        // float maxRotation = 45f;
        // Quaternion targetRotation = Quaternion.LookRotation(targetDirection - Cannon.position);
        // Quaternion newRotation = Quaternion.RotateTowards(Cannon.rotation, targetRotation, maxRotation * Time.deltaTime);


        // //Vector3 newDirection = Vector3.RotateTowards(Cannon.forward, targetDirection, maxRotation * Mathf.Deg2Rad, 0f);
        

        return new Vector2(BaseTurn, BarrelAngle).normalized;
>>>>>>> Stashed changes
    }

    public bool WantToZoom() {
        return false;
    }
    
    public bool WantToFire() {
<<<<<<< Updated upstream
        float fuzzy_forward = Vector3.Dot(-transform.forward, Vector3.Normalize(Target - transform.position));
        return fuzzy_forward > 0.9f;
=======
        Vector2 cannonAim = GetTurretInput();
        Vector3 targetDirection = TargetFinder.Target - Cannon.position;
        float accuracy = Vector2.Dot(cannonAim, new Vector2(targetDirection.x, targetDirection.z).normalized);
        return accuracy > 0.8f;
>>>>>>> Stashed changes
    }

}
