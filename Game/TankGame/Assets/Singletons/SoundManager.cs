using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundAsset {
    Explosion, Collision, DriveMode, CombatMode, Boost, theme1, theme2, theme3, theme4
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioClip ExplosionAudio;
    public AudioClip CollisionAudio;
    public AudioSource DriveModeA;
    public AudioSource CombatModeA;
    public AudioSource Boost;

    public AudioSource theme1;
    public AudioSource theme2;
    public AudioSource theme3;
    public AudioSource theme4;

    private AudioSource _playing;

    void Update() {
        if (theme1 != null) theme1.volume = Globals.Instance.MusicAudio;
        if (theme2 != null) theme2.volume = Globals.Instance.MusicAudio;
        if (theme3 != null) theme3.volume = Globals.Instance.MusicAudio;
        if (theme4 != null) theme4.volume = Globals.Instance.MusicAudio;

        if (DriveModeA != null) DriveModeA.volume = Globals.Instance.SFXAudio;
        if (CombatModeA != null) CombatModeA.volume = Globals.Instance.SFXAudio;
        if (Boost != null) Boost.volume = Globals.Instance.SFXAudio;
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

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void _playTheme(AudioSource theme) {
        Fade(_playing);
        _playing = theme;
        theme?.Play();
    }

    // Todo(Zack): Cache sounds each frame to stop sound from
    // stacking on top of each other
    public void PlaySound(SoundAsset asset, Vector3 position) {
        switch(asset) {
            case SoundAsset.Collision: AudioSource.PlayClipAtPoint(CollisionAudio, position, Globals.Instance.SFXAudio); break;
            case SoundAsset.Explosion: AudioSource.PlayClipAtPoint(ExplosionAudio, position, Globals.Instance.SFXAudio); break;
            case SoundAsset.DriveMode: DriveModeA?.Play(); break;
            case SoundAsset.CombatMode: CombatModeA?.Play(); break;
            case SoundAsset.Boost: Boost?.Play(); break;
            case SoundAsset.theme1: _playTheme(theme1); break;
            case SoundAsset.theme2: _playTheme(theme2); break;
            case SoundAsset.theme3: _playTheme(theme3); break;
            case SoundAsset.theme4: _playTheme(theme4); break;
        }
    }

    public void Fade(AudioSource audiosource)
    {
        audiosource?.Stop();
    }
}
