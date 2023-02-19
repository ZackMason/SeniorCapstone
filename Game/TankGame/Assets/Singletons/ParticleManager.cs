using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }
    
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
            Debug.Assert(_particles);        
        } 
    }

    public GameObject _particles;

    [Range(0, 1000)]
    public int MaxParticles;

    public bool TrySpawn(GameObject particleSystem, Vector3 position) {
        if (_particles.transform.childCount < MaxParticles) {
            var particle = Instantiate(particleSystem, position, Quaternion.identity);
            particle.transform.SetParent(_particles.transform);
            return true;
        }
        return false;
    }
}
