using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDig : MonoBehaviour
{
    private bool _canExplode = true;

    private LayerMask _layerToCheck = 11;
    private Collider[] ColliderArray = new Collider[10];

    private bool _isSphereTriggerCollidingWithLayer() {
        int num_colliders = Physics.OverlapSphereNonAlloc(transform.position, 1.7f, ColliderArray, _layerToCheck);
        // print($"colliders: {num_colliders}");
        return num_colliders > 0;
    }

    private bool _isColliding() => _isSphereTriggerCollidingWithLayer();

    IEnumerator _resetExplosion() {
        yield return new WaitForSeconds(0.5f);
        _canExplode = true;
    }

    void FixedUpdate() {
        if (_canExplode && _isColliding()) {
            // print("boom");
            _canExplode = false;
            ExplosionManager.Instance.SpawnBossExplosion(transform.position, 4.0f, 5000f, 12f*50f);
            StartCoroutine(_resetExplosion());
        }
    }
}
