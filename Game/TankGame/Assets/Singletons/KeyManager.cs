using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }
    
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

    public void PickupKey(string keyName) {
        UpdateBarriers(keyName);
    }

    private void UpdateBarriers(string keyName) {
        LevelBarrier[] barriers = FindObjectsOfType(typeof(LevelBarrier)) as LevelBarrier[];
        foreach(LevelBarrier barrier in barriers) {
            if (keyName == barrier.KeyName) {
                barrier.OpenBarrier();
            }
        }
    }
}
