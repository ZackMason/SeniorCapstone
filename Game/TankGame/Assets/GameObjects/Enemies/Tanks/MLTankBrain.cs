using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MLTankBrain : Agent, ITankBrain
{
    [SerializeField] private GameObject _tankPrefab;
    [SerializeField] private GameObject _tank;
    [SerializeField] private EnemyTankBrain _trainingBrain;

    [SerializeField] private Vector2 _drive;
    [SerializeField] private Vector2 _turret;
    [SerializeField] private float _boost;
    [SerializeField] private float _fire;

    public Transform Cannon; // Note(Zack): tank_turret
    public Faction Faction;
    [SerializeField] private TargetFinder TargetFinder;

    [SerializeField] LayerMask _rayLayer = ~0;
    [SerializeField]
    private bool _useML = true;

    void Start() {
        Faction = GetComponent<Faction>();
    }

    void FixedUpdate() {
        if (_tank == null) {
            _tank = Instantiate(_tankPrefab, transform.position, Quaternion.identity);
            _tank.GetComponent<EnemyTankBrain>().MLBrain = this;
            Cannon = _tank.transform.Find("tank_head/tank_turret");
            TargetFinder = _tank.GetComponent<TargetFinder>();
            Faction = _tank.GetComponent<Faction>();
            HEWeapon weapon = _tank.GetComponentInChildren<HEWeapon>();
            weapon.MLBrain = this;
            _tank.transform.SetParent(transform);
            
            AddReward(-10);
            EndEpisode();
        } else {
            AddReward(Time.fixedDeltaTime * 0.1f);
        }
    }

    public void OnKill(Faction faction) {
        if (faction.ID != Faction.ID) {
            Debug.Log("Kill Rewarded");
            AddReward(100);
        } else {
            Debug.Log("Tank Suicided");
            // AddReward(-200);
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        _drive.x = actions.ContinuousActions[0];
        _drive.y = actions.ContinuousActions[1];
        _turret.x = actions.ContinuousActions[2];
        // _turret.y = actions.ContinuousActions[3];
        _fire = actions.ContinuousActions[4];
        if (_fire > 0.0f) {
            AddReward(_computeAccuracy() > 0.8f ? 0.001f : 0.0f);
        }
    }

    public override void CollectObservations(VectorSensor sensor) {
        if (_tank != null) {
            sensor.AddObservation(_tank.transform.position);
        } else {
            sensor.AddObservation(transform.position);
        }
        sensor.AddObservation(TargetFinder.Target);
        sensor.AddObservation(TargetFinder.MoveTarget);
        sensor.AddObservation(Cannon.transform.forward);
        sensor.AddObservation((_tank.transform.position - TargetFinder.Target).magnitude);
        sensor.AddObservation((_tank.transform.position - TargetFinder.MoveTarget).magnitude);

        int rayCount = 10;
        for (int i = 0; i < rayCount; i++) {
            float angle = 2.0f * Mathf.PI / rayCount;
            Vector3 dir = new Vector3(
                Mathf.Cos(angle), 0.0f, Mathf.Sin(angle)
            );
            RaycastHit hit;
            Physics.Raycast(_tank.transform.position, dir, out hit, Mathf.Infinity, _rayLayer);
            sensor.AddObservation(hit.distance);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        base.Heuristic(actionsOut);
    }

    // Todo(Zack): Add raycast to avoid running into walls
    // Maybe boost if we are stuck
    public Vector2 GetDriveInput() {
        if (_useML) {
            return _drive;
        }
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
        if (_useML) {
            return _turret;
        }
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

    private float _computeAccuracy() { 
        if (Cannon == null) { return 0.0f; }
        Vector3 cannonAim = -Cannon.forward;
        Vector3 targetDirection = (TargetFinder.Target - Cannon.position).normalized;
        return Vector3.Dot(cannonAim, targetDirection);
    }
    
    public bool WantToFire() {
        if (_useML) {
            return _fire > 0.0f;
        }
        Vector3 cannonAim = -Cannon.forward;
        Vector3 targetDirection = (TargetFinder.Target - Cannon.position).normalized;
        float accuracy = Vector3.Dot(cannonAim, targetDirection);
        return accuracy > 0.8f;
    }
}
