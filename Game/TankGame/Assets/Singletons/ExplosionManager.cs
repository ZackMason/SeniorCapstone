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

    public void SpawnExplosion(Vector3 position, float radius, float power, float damage) {
        ParticleManager.Instance.TrySpawn(ExplosionParticles, position);
        SoundManager.Instance.PlaySound(SoundAsset.Explosion, position);
        
        var colliders = Physics.OverlapSphere(position, radius);

        foreach (Collider proximity in colliders) {
            Rigidbody body = proximity.GetComponent<Rigidbody>();
            Health health = proximity.GetComponent<Health>();

            if (health != null) {
                float falloff = 1.0f / Mathf.Max(1.0f, (proximity.transform.position - position).sqrMagnitude);
                health.Damage(Random.Range(damage*0.5f, damage*1.5f) * falloff);
            }

            if (body != null) {
                body.AddExplosionForce(power, position, radius);
            } else {
                // Debug.Log("No Rigidbody on collider");
            }
        }
    }
}
