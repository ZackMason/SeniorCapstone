using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEWeapon : MonoBehaviour, IWeapon
{
    public AudioSource eaudio;
    private float _fireTime;

    [Range(-1, 1)]
    public float ShootDir;
    [Range(0, 2)]
    public float FireRate;

    [Range(0, 20)]
    public float ExplosionRadius;

    [Range(0, 1000)]
    public float ExplosionPower;

    [Range(0, 200)]
    public float DamagePower;

    public GameObject ExplosionParticles;

    public void Fire() {
        if (_fireTime > 0.0f) {
            return;
        }
        Vector3 rayDirection = ShootDir * transform.TransformDirection(Vector3.forward);
        Vector3 rayOrigin = transform.position;

        Debug.DrawRay(rayOrigin, rayDirection, Color.red); 

        int layerMask = ~0;

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, layerMask)) {
            ParticleManager.Instance.TrySpawn(ExplosionParticles, hit.point);
            
            _fireTime = FireRate;
            var health = hit.transform.gameObject.GetComponent<Health>();
            if (health != null) {
                health.Damage(Random.Range(DamagePower*0.5f, DamagePower*1.5f));
            }
            
            ExplosionManager.Instance.SpawnExplosion(hit.point, ExplosionRadius, ExplosionPower);
            if (eaudio != null) {
                eaudio.Play();
            }
        }
    }

    public void FixedUpdate() {
        _fireTime -= Time.fixedDeltaTime;
    }
}
