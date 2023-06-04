using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }

    // Graphics
    public float AAQuality { 
        set {
            Camera[] cameras = FindObjectsOfType(typeof(Camera)) as Camera[];
            
            foreach(Camera camera in cameras) {
                UniversalAdditionalCameraData uacd = camera.GetUniversalAdditionalCameraData();
                if (value == 0f) {
                    uacd.antialiasing = AntialiasingMode.None;
                    print("no aa");
                } else if (value == 1f) {
                    print("fxaa");
                    uacd.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                } else if (value == 2f) {
                    print("smaa");
                    uacd.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;

                }
            }
        }
    }

    // Gameplay
    public float MouseSensitivity { get; set; } = 1f;
    public float CameraShake { get; set; } = 1f;

    // Audio
    public float MainAudio { get; set; } = 1f;

    private float _musicAudio = 1f;
    public float MusicAudio { 
        get {
            return _musicAudio * MainAudio;
        }
        set {
            _musicAudio = value;
        }
    }

    private float _sfxAudio = 1f;
    public float SFXAudio { 
        get {
            return _sfxAudio * MainAudio;
        }
        set {
            _sfxAudio = value;
        }
    }
    
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
