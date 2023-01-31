using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

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


    [Range(60.0f, 60.0f*5.0f)]
    public float TimeLimit;

    private float _gameTime = 0.0f;
    
    void FixedUpdate() {
        _gameTime += Time.deltaTime;

        if (_gameTime > TimeLimit) {
            Debug.Log("Game Time Limit Reached! (nothing happens yet)");
            Destroy(this);
        }
    }
    
}
