using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundAsset {
    Explosion, Collision
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource ExplosionAudio;
    public AudioSource CollisionAudio;

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

    public void PlaySound(SoundAsset asset) {
        switch(asset) {
            case SoundAsset.Collision: CollisionAudio.Play(); break;
            case SoundAsset.Explosion: ExplosionAudio.Play(); break;
        }
    }
}
