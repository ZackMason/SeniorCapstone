using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public GameObject ExplosionParticles;
    
    public static ExplosionManager Instance { get; private set; }
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

    public void SpawnExplosion(Vector3 position, float radius, float power) {
        ParticleManager.Instance.TrySpawn(ExplosionParticles, position);
        SoundManager.Instance.PlaySound(SoundAsset.Explosion);
        
        var colliders = Physics.OverlapSphere(position, radius);

        foreach (Collider proximity in colliders) {
            Rigidbody body = proximity.GetComponent<Rigidbody>();

            if (body != null) {
                body.AddExplosionForce(power, position, radius);
            } else {
                // Debug.Log("No Rigidbody on collider");
            }
        }
    }
}
