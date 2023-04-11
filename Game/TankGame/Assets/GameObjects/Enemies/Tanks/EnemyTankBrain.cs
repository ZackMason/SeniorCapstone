using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankBrain : MonoBehaviour, ITankBrain
{
    private TargetFinder TargetFinder;
    
    public Transform Cannon; // Note(Zack): tank_turret

    void Start() {
        TargetFinder = GetComponent<CombatPositionFinder>();
    }

    // Todo(Zack): Add raycast to avoid running into walls
    // Maybe boost if we are stuck
    public Vector2 GetDriveInput() {
        float fuzzy_forward = Vector3.Dot(-transform.forward, Vector3.Normalize(TargetFinder.MoveTarget - transform.position));
        float fuzzy_right = Vector3.Dot(-transform.right, Vector3.Normalize(TargetFinder.MoveTarget - transform.position));

        return new Vector2(
            Mathf.Clamp(fuzzy_right*2.0f, -1.0f, 1.0f),
            fuzzy_forward
        );
    }

    public bool GetAirbrake() {
        return false;
    }

    public float GetBoost() {
        return 0.0f;
    }

    public Vector2 GetTurretInput() {
        var target = TargetFinder.Target;
        var dir = (target - transform.position).normalized;
        var turretDir = Cannon.forward;
        var turretSideDir = Cannon.right;
        var fuzzyAim = Vector3.Dot(turretSideDir, dir);
        fuzzyAim = Mathf.Clamp(Mathf.Sqrt(Mathf.Abs(fuzzyAim)) * Mathf.Sign(fuzzyAim) + 
                    0.5f * Mathf.Pow(fuzzyAim, 3), -1.0f, 1.0f);

        return new Vector2(fuzzyAim * 3.0f, 0.0f);
    }
    
    public bool WantToSwitchMode(){
        return false;
    }
    public bool WantToZoom() {
        return false;
    }
    
    public bool WantToFire() {
        Vector3 cannonAim = -Cannon.forward;
        Vector3 targetDirection = (TargetFinder.Target - Cannon.position).normalized;
        float accuracy = Vector3.Dot(cannonAim, targetDirection);
        return accuracy > 0.8f;
    }

}
