using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
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
    public int PlayerScore = 0;
    public int PlayerPenalty = 0;

        public void OnShelfDamaged() {
        PlayerPenalty += 100; // todo(zack): Adjust score
    }
}
