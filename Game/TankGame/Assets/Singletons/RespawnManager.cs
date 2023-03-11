using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerPrefab;

    [Range(1, 60)]
    public float CheckpointTime;

    private Vector3 _lastPlayerPosition;
 
    private const float _OFF = -1.0f;
    private float _spawnTimer = _OFF;
    private float _checkpointTimer = 0.0f;

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

    bool _isOnGround(Vector3 pos) {
        return true;
        RaycastHit hit;
        return Physics.Raycast(pos, Vector3.down, out hit, 4.0f, 0);
    }

    // todo(zack): move out of update / use timer
    void FixedUpdate()
    {
        _checkpointTimer -= Time.fixedDeltaTime;
        if (_spawnTimer > 0.0f) {
            _spawnTimer -= Time.fixedDeltaTime;
        }

        if (Player == null) { // player died
            if (_spawnTimer == _OFF) {
                _spawnTimer = 1.0f;
            }
        } else { // player is alive
            if (_checkpointTimer < 0.0f) {
                if (_isOnGround(Player.transform.position)) {
                    _lastPlayerPosition = Player.transform.position;
                    _checkpointTimer = CheckpointTime;
                }
            }
        }

        if (_spawnTimer < 0.0f && _spawnTimer != _OFF) {
            _spawnTimer = _OFF;
            Player = GameObject.Instantiate(PlayerPrefab, _lastPlayerPosition, Quaternion.identity);
        }
    }
}
