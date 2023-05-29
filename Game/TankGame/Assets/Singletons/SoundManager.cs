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

    // Todo(Zack): Cache sounds each frame to stop sound from
    // stacking on top of each other
    public void PlaySound(SoundAsset asset, Vector3 position) {
        switch(asset) {
            case SoundAsset.Collision: AudioSource.PlayClipAtPoint(CollisionAudio, position); break;
            case SoundAsset.Explosion: AudioSource.PlayClipAtPoint(ExplosionAudio, position); break;
            case SoundAsset.DriveMode: DriveModeA.Play(); break;
            case SoundAsset.CombatMode: CombatModeA.Play(); break;
            case SoundAsset.Boost: Boost?.Play(); break;
            case SoundAsset.theme1: theme1.Play(); break;
            case SoundAsset.theme2: Fade(theme1); theme2.Play(); break;
            case SoundAsset.theme3: Fade(theme2); theme3.Play(); break;
            case SoundAsset.theme4: Fade(theme3); theme4.Play(); break;
        }
    }

    public void Fade(AudioSource audiosource)
    {
        audiosource.Stop();
    }
}
