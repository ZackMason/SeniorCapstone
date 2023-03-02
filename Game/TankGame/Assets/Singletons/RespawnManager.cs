using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerPrefab;

    private Vector3 _lastPlayerPosition;
    private float _spawnTimer = -1.0f;

    void Start()
    {
        
    }


    // todo(zack): move out of update / use timer
    void FixedUpdate()
    {
        if (_spawnTimer > 0.0f) {
            _spawnTimer -= Time.fixedDeltaTime;
        }

        if (Player == null) {
            if (_spawnTimer == -1.0f) {
                _spawnTimer = 1.0f;
            }
        } else {
            _lastPlayerPosition = Player.transform.position;
        }

        if (_spawnTimer < 0.0f && _spawnTimer != -1.0f) {
            _spawnTimer = -1.0f;
            Player = GameObject.Instantiate(PlayerPrefab, _lastPlayerPosition, Quaternion.identity);
        }
    }
}
