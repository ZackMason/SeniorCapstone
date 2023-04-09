using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HEProjectile : MonoBehaviour {
    private Rigidbody _rb;

    [Range(0, 100)]
    public float BounceChance = 10.0f;

    [Range(0, 20)]
    public float ExplosionRadius;

    [Range(0, 1000)]
    public float ExplosionPower;

    [Range(0, 200)]
    public float DamagePower;
    
    [Range(0, 80000)]
    public float LaunchPower = 4300.0f;

    private float _lifeTime = 10.0f;

    IEnumerator KillProjectile() {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(this.gameObject);
    }
    
    void Start() {
        _rb = GetComponent<Rigidbody>();
        _rb.AddRelativeForce(new Vector3(0, 0, -LaunchPower));
        StartCoroutine(KillProjectile());
    }

    public void OnCollisionEnter(Collision col) {
        ExplosionManager.Instance.SpawnExplosion(
            col.contacts[0].point, 
            ExplosionRadius, 
            ExplosionPower, 
            DamagePower
        );

        //Note(Zack): If we roll over bounce change, delete the projectile
        // IE, 25% Bounce Chance -> 75% are deleted
        if (Random.Range(0.0f, 100.0f) > BounceChance) {
            Destroy(gameObject);
        }
    }
}
