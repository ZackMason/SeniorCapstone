using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MLTankBrain : Agent, ITankBrain
{
    [SerializeField] private int _faction;
    [SerializeField] private GameObject _tankPrefab;
    [SerializeField] private GameObject _tank;
    [SerializeField] private EnemyTankBrain _trainingBrain;

    [SerializeField] private Vector2 _drive;
    [SerializeField] private Vector2 _turret;
    [SerializeField] private float _boost;
    [SerializeField] private float _fire;

    public Transform Body; // Note(Zack): tank_body
    public Transform Cannon; // Note(Zack): tank_turret
    public Faction Faction;
    [SerializeField] private TargetFinder TargetFinder;

    [SerializeField] LayerMask _rayLayer = ~0;
    [SerializeField]
    private bool _useML = false;

    void Start() {
        Faction = GetComponent<Faction>();
    }

    void FixedUpdate() {
        if (_tank == null) {
            _tank = Instantiate(_tankPrefab, transform.position, Quaternion.identity);
            _tank.GetComponent<EnemyTankBrain>().MLBrain = this;
            _tank.GetComponent<Faction>().ID = _faction;
            Body = _tank.transform.Find("tank_body");
            Cannon = _tank.transform.Find("tank_head/tank_turret");
            TargetFinder = _tank.GetComponent<TargetFinder>();
            Faction = _tank.GetComponent<Faction>();
            HEWeapon weapon = _tank.GetComponentInChildren<HEWeapon>();
            weapon.MLBrain = this;
            _tank.transform.SetParent(transform);
            _tank.transform.localPosition = new Vector3(
                UnityEngine.Random.Range(-20.0f, 40.0f), 
                0f,
                UnityEngine.Random.Range(-20.0f, 40.0f));
            
            AddReward(-10);
            EndEpisode();
        } else {
            // AddReward(-Mathf.Abs(_drive.x*Time.fixedDeltaTime*50f));
            AddReward(Time.fixedDeltaTime * 0.1f);
            AddReward(_computeAccuracy());
            // AddReward(Time.fixedDeltaTime * 0.1f / Mathf.Max((TargetFinder.MoveTarget-_tank.transform.position).magnitude, 0.1f));
        }
    }

    public void OnKill(Faction faction) {
        if (faction.ID != Faction.ID) {
            Debug.Log("Kill Rewarded");
            AddReward(100);
        } else {
            Debug.Log("Tank Shot Itself");
            // AddReward(-50);
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        _drive.x = actions.ContinuousActions[0];
        _drive.y = actions.ContinuousActions[1];
        _turret.x = actions.ContinuousActions[2];
        _fire = actions.ContinuousActions[3];
        // _turret.y = actions.ContinuousActions[3];

        // Note(Zack):
        // Reward based off predictions of how it should act
        // in order to try to speed up training
        // if (_fire > 0.0f) {
        //     AddReward(_computeAccuracy() > 0.8f ? 0.001f : 0.0f);
        // }
        // prefer driving forward
        // AddReward(Vector2.Dot(_drive, new Vector2(0.0f, 1.0f)) * 0.01f);
        // AddReward(Vector2.Dot(_drive, GetDriveInput()) * 0.01f);
        // AddReward(Vector2.Dot(_turret, GetTurretInput()) * 0.01f);
    }

    public override void CollectObservations(VectorSensor sensor) {
        if (_tank != null && 
            Body != null && 
            Cannon != null) {
            sensor.AddObservation(_tank.transform.position);
            sensor.AddObservation(-Body.transform.forward);
            sensor.AddObservation(Cannon.transform.forward);
            sensor.AddObservation((_tank.transform.position - TargetFinder.Target).magnitude);
            sensor.AddObservation((_tank.transform.position - TargetFinder.MoveTarget).magnitude);
            sensor.AddObservation(Vector3.Dot(Cannon.transform.right, (_tank.transform.position - TargetFinder.MoveTarget).normalized));
            sensor.AddObservation(TargetFinder.Target);
            sensor.AddObservation(TargetFinder.MoveTarget);
            int rayCount = 10;
            for (int i = 0; i < rayCount; i++) {
                float angle = i * 2.0f * Mathf.PI / rayCount;
                var rayOrigin = _tank.transform.position;
                
                Vector3 dir = new Vector3(
                    Mathf.Cos(angle), 0.0f, Mathf.Sin(angle)
                );
                
                float clostestDistance = 1000f;
                if (Physics.Raycast(rayOrigin, dir, out RaycastHit hit, clostestDistance, _rayLayer)) {
                    float distance = hit.distance;
                    clostestDistance = Mathf.Min(clostestDistance, distance);
                }
                // Debug.DrawRay(rayOrigin, dir * clostestDistance, clostestDistance == 1000f ? Color.white : Color.red);
                sensor.AddObservation(clostestDistance);
            }
        } else {
            sensor.AddObservation(transform.position);
            sensor.AddObservation(Vector3.zero);
            sensor.AddObservation(Vector3.zero);
            sensor.AddObservation(Vector3.zero);
            sensor.AddObservation(Vector3.zero);
            sensor.AddObservation(-1f);
            sensor.AddObservation(-1f);
            sensor.AddObservation(-1f);
            for (int i = 0; i < 10; i++) {
                sensor.AddObservation(0f);
            }
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
            return _turret * 3.0f;
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
