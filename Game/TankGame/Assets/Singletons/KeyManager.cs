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

        _collectedKeys = new HashSet<string>();
    }

    
    private HashSet<string> _collectedKeys;

    public void PickupKey(string keyName) {
        // Debug.Assert(_collectedKeys.Contains(keyName) == false); // Dont add duplicate keynames
        _collectedKeys.Add(keyName);

        UpdateBarriers();
    }

    private void UpdateBarriers() {
        LevelBarrier[] barriers = FindObjectsOfType(typeof(LevelBarrier)) as LevelBarrier[];
        foreach(LevelBarrier barrier in barriers) {
            if (_collectedKeys.Contains(barrier.KeyName)) {
                barrier.OpenBarrier();
            }
        }
    }
}
