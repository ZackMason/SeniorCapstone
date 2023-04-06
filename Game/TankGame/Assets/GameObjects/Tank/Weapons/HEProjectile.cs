using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEProjectile : MonoBehaviour {
    private Rigidbody _rb;

    [Range(0, 20)]
    public float ExplosionRadius;

    [Range(0, 1000)]
    public float ExplosionPower;

    [Range(0, 200)]
    public float DamagePower;
    
    [Range(0, 80000)]
    public float LaunchPower = 4300.0f;
    
    void Start() {
        _rb = GetComponent<Rigidbody>();

        Debug.Assert(_rb);

        _rb.AddRelativeForce(new Vector3(0, 0, -LaunchPower));
    }

    public void OnCollisionEnter(Collision col) {
        var health = col.transform.gameObject.GetComponent<Health>();
        if (health != null) {
            health.Damage(Random.Range(DamagePower*0.5f, DamagePower*1.5f));
        }
        
        ExplosionManager.Instance.SpawnExplosion(col.contacts[0].point, ExplosionRadius, ExplosionPower);
        SoundManager.Instance.PlaySound(SoundAsset.Explosion);
    }
}
