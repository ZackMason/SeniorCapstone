using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEWeapon : MonoBehaviour, IWeapon
{
    private float _fireTime;

    [Range(0, 2)]
    public float FireRate;

    public GameObject ExplosionParticles;

    public void Fire() {
        if (_fireTime > 0.0f) {
            return;
        }
        Vector3 rayDirection = transform.TransformDirection(Vector3.forward);
        Vector3 rayOrigin = transform.position;

        Debug.DrawRay(rayOrigin, rayDirection, Color.red); 

        int layerMask = ~0;

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, -rayDirection, out hit, Mathf.Infinity, layerMask)) {
            ParticleManager.Instance.TrySpawn(ExplosionParticles, hit.point);
            _fireTime = FireRate;
        }
    }

    public void FixedUpdate() {
        _fireTime -= Time.fixedDeltaTime;
    }
}
