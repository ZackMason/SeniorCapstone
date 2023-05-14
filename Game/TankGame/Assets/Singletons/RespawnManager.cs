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
    void Start() => SetRespawnTransform(Player?.transform ?? new Transform());
    
    bool _isOnGround(Vector3 pos) {
        return true;
        // RaycastHit hit;
        // return Physics.Raycast(pos, Vector3.down, out hit, 4.0f, 0);
    }

    IEnumerator RespawnPlayer() {
        yield return new WaitForSeconds(RespawnTime);
        Destroy(Player);
        Player = GameObject.Instantiate(PlayerPrefab, _lastPlayerPosition, _lastPlayerRotation);
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
        if ((Player?.GetComponent<HoverTankController>()?.IsAlive() ?? true) == false &&
            RespawnCoroutine == null
        ) { // player died
            Player?.GetComponent<HoverTankController>()?.OverrideDeathTimer();
            RespawnCoroutine = StartCoroutine(RespawnPlayer());
        } 
    }
}
