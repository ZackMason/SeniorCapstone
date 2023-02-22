using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankBrain : MonoBehaviour, ITankBrain
{
    public Vector3 Target;

    private Transform _transform;

    void Start() {
        _transform = GetComponent<Transform>();
    }

    public Vector2 GetDriveInput() {
        float fuzzy_forward = Vector3.Dot(-_transform.forward, Vector3.Normalize(Target - _transform.position));
        float fuzzy_right = Vector3.Dot(-_transform.right, Vector3.Normalize(Target - _transform.position));

        return new Vector2(
            fuzzy_right,
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
        float fuzzy_forward = Vector3.Dot(-_transform.forward, Vector3.Normalize(Target - _transform.position));
        return fuzzy_forward > 0.9f;
    }

}
