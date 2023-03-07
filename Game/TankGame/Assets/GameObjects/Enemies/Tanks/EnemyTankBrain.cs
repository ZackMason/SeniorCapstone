using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankBrain : MonoBehaviour, ITankBrain
{
    public Vector3 Target;
    public Transform Cannon;

    
    public Vector2 GetDriveInput() {
        float fuzzy_forward = Vector3.Dot(-transform.forward, Vector3.Normalize(Target - transform.position));
        float fuzzy_right = Vector3.Dot(-transform.right, Vector3.Normalize(Target - transform.position));

        return new Vector2(
            Mathf.Clamp(fuzzy_right*2.0f, -1.0f, 1.0f),
            fuzzy_forward
        );
    }

    public Vector2 GetTurretInput() {
        // TODO(ZACK): do turret
        Vector3 targetDirection = Vector3.Normalize(Target - transform.position);
        float maxRotation = 90f;
        Vector3 newDirection = Vector3.RotateTowards(Cannon.forward, targetDirection, maxRotation * Mathf.Deg2Rad, 0f);
        return new Vector2(newDirection.x, newDirection.y).normalized;
    }

    public bool WantToZoom() {
        return false;
    }
    
    public bool WantToFire() {
        Vector2 cannonAim = GetTurretInput();
        return cannonAim.y > 0.9f;
    }

}
