using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundAsset {
    Explosion, Collision, DriveMode, CombatMode
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioClip ExplosionAudio;
    public AudioClip CollisionAudio;
    public AudioSource DriveModeA;
    public AudioSource CombatModeA;

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

    // Todo(Zack): Cache sounds each frame to stop sound from
    // stacking on top of each other
    public void PlaySound(SoundAsset asset, Vector3 position) {
        switch(asset) {
            case SoundAsset.Collision: AudioSource.PlayClipAtPoint(CollisionAudio, position); break;
            case SoundAsset.Explosion: AudioSource.PlayClipAtPoint(ExplosionAudio, position); break;
            case SoundAsset.DriveMode: DriveModeA.Play(); break;
            case SoundAsset.CombatMode: CombatModeA.Play(); break;
        }
    }
}
