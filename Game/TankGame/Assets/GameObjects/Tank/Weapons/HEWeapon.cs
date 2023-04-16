using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEWeapon : MonoBehaviour, IWeapon
{
    public GameObject ProjectilePrefab;
    public AudioSource eaudio;
    private float _fireTime;

    public bool MakeNoise = false;

    [Range(-1, 1)]
    public float AimDir;

    [Range(0, 2)]
    public float FireRate;

    [Range(0, 20)]
    public float ExplosionRadius;

    [Range(0, 1000)]
    public float ExplosionPower;

    [Range(0, 200)]
    public float DamagePower;

    public GameObject ExplosionParticles;

    public MLTankBrain MLBrain;

    public void Fire() {
        if (_fireTime > 0.0f) {
            return;
        }
        Vector3 rayDirection = AimDir * transform.TransformDirection(Vector3.forward);
        Vector3 rayOrigin = transform.position;
        
        var projectile = Instantiate(ProjectilePrefab, rayOrigin, transform.rotation);
        if (MLBrain != null) {
            projectile.GetComponent<HEProjectile>().OnKill += MLBrain.OnKill;
        }
        
        Debug.DrawRay(rayOrigin, rayDirection, Color.red); 

        _fireTime = FireRate;
    }

    public void FixedUpdate() {
        _fireTime -= Time.fixedDeltaTime;
    }
}
