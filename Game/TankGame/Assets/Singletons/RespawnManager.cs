    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerPrefab;

    [Range(1, 60)]
    public float CheckpointTime;

    [Range(1, 30)]
    public float RespawnTime;

    private Vector3 _lastPlayerPosition;
    private Quaternion _lastPlayerRotation;
 
    private Coroutine RespawnCoroutine;

    public static RespawnManager Instance { get; private set; }

    private float _ignoreTimer = 0f;
    public bool ShouldIgnorePlayer() => _ignoreTimer > 0f;
    
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);
        }
        else 
        { 
            Instance = this;
        }
    }

    public void SetRespawnPosition(Vector3 pos) => _lastPlayerPosition = pos;
    public void SetRespawnRotation(Quaternion rot) => _lastPlayerRotation = rot;
    public void SetRespawnTransform(Transform t) {
        SetRespawnPosition(t.position);
        SetRespawnRotation(t.rotation);
    }
    void Start() => SetRespawnTransform(Player?.transform ?? transform);
    
    bool _isOnGround(Vector3 pos) {
        return true;
    }

    public void KillPlayer() {
        if (Player) {
            var health = Player.GetComponentInChildren<Health>();
            health.Damage(100000000);
        }
    }

    IEnumerator RespawnPlayer() {
        yield return new WaitForSeconds(RespawnTime);
        _ignoreTimer = 4f;
        Destroy(Player);
        Player = GameObject.Instantiate(PlayerPrefab, _lastPlayerPosition, _lastPlayerRotation);
        Player.GetComponent<HoverTankController>().BoostCooldownTime = 2;
        Player.GetComponent<HoverTankController>()._boostTimer = 0;
        RespawnCoroutine = null;
    }

    IEnumerator SetCheckpoint() {
        while(true) {
            if (_isOnGround(Player.transform.position) && RespawnCoroutine == null) {
                _lastPlayerPosition = Player.transform.position;
                _lastPlayerRotation = Player.transform.rotation;
            }
            yield return new WaitForSeconds(CheckpointTime);
        }
    }

    void FixedUpdate()
    {
        _ignoreTimer -= Time.fixedDeltaTime;

        if ((Player?.GetComponent<HoverTankController>()?.IsAlive() ?? true) == false &&
            RespawnCoroutine == null
        ) { // player died
            Player?.GetComponent<HoverTankController>()?.OverrideDeathTimer();
            RespawnCoroutine = StartCoroutine(RespawnPlayer());
        } 
    }
}
