using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOptions : MonoBehaviour
{
    [SerializeField] private Slider _mouseSensitivity;
    [SerializeField] private Slider _Antialiasing;
    [SerializeField] private Slider _CameraShake;
    [SerializeField] private Slider _mainAudio;
    [SerializeField] private Slider _musicAudio;
    [SerializeField] private Slider _sfxAudio;
    
    void Start()
    {
        _mouseSensitivity.value = Globals.Instance.MouseSensitivity;
        _CameraShake.value = Globals.Instance.CameraShake;
        _Antialiasing.value = Globals.Instance.AAQuality;

        _mainAudio.value = Globals.Instance.MainAudio;
        _sfxAudio.value = Globals.Instance.SFXAudio / Globals.Instance.MainAudio;
        _musicAudio.value = Globals.Instance.MusicAudio / Globals.Instance.MainAudio;
    }
}
