using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundAsset {
    Explosion, Collision
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioClip ExplosionAudio;
    public AudioClip CollisionAudio;

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

    public void PlaySound(SoundAsset asset, Vector3 position) {
        switch(asset) {
            case SoundAsset.Collision: AudioSource.PlayClipAtPoint(CollisionAudio, position); break;
            case SoundAsset.Explosion: AudioSource.PlayClipAtPoint(ExplosionAudio, position); break;

        }
    }
}
