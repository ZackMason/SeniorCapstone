using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }

    // Graphics
    public float AAQuality { 
        get {
            return PlayerPrefs.GetFloat("AA", 0f);
        }
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
                PlayerPrefs.SetFloat("AA", value);
                PlayerPrefs.Save();
            }
        }
    }

    // Gameplay
    public float MouseSensitivity {
        get {
            return PlayerPrefs.GetFloat("MouseSensitivity", 1f);
        }
        set {
            PlayerPrefs.SetFloat("MouseSensitivity", value);
            PlayerPrefs.Save();
        }
    }
    public float CameraShake {
        get {
            return PlayerPrefs.GetFloat("CameraShake", 1f);
        }
        set {
            PlayerPrefs.SetFloat("CameraShake", value);
            PlayerPrefs.Save();
        }
    }

    // Audio
    public float MainAudio {
        get {
            return PlayerPrefs.GetFloat("MainAudio", 1f);
        }
        set {
            PlayerPrefs.SetFloat("MainAudio", value);
            PlayerPrefs.Save();
        }
    }

    public float MusicAudio {
        get {
            return PlayerPrefs.GetFloat("MusicAudio", 1f) * MainAudio;
        }
        set {
            PlayerPrefs.SetFloat("MusicAudio", value);
            PlayerPrefs.Save();
        }
    }

    public float SFXAudio {
        get {
            return PlayerPrefs.GetFloat("SFXAudio", 1f) * MainAudio;
        }
        set {
            PlayerPrefs.SetFloat("SFXAudio", value);
            PlayerPrefs.Save();
        }
    }
    
    private void Awake() { 
        if (Instance != null && Instance != this) { 
            Destroy(this); 
        } else { 
            Instance = this; 
        } 
    }
}
