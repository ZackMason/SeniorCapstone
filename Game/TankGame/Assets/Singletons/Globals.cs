using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }

    // Gameplay
    public float MouseSensitivity { get; set; } = 1f;

    // Audio
    public float MainAudio { get; set; } = 1f;
    public float MusicAudio { get; set; } = 1f;
    public float SFXAudio { get; set; } = 1f;
    
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

}
