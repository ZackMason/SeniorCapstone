using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void _kill(GameObject go) {
        if (go?.tag != "Boss") {
            go?.GetComponent<Health>()?.Damage(1000000);
        }
    }

    void OnCollisionEnter(Collision collision) {
        _kill(collision.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        _kill(other.gameObject);
        _kill(other.attachedRigidbody?.gameObject);
    }
}
