using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankBrain : MonoBehaviour, ITankBrain
{
    public Vector3 Target;

    
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
        return new Vector2(
            0.0f, 0.0f
        );
    }

    public bool WantToZoom() {
        return false;
    }
    
    public bool WantToFire() {
        float fuzzy_forward = Vector3.Dot(-transform.forward, Vector3.Normalize(Target - transform.position));
        return fuzzy_forward > 0.9f;
    }

}
