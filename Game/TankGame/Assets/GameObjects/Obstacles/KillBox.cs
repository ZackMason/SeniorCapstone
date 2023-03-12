using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void _kill(GameObject go) {
        Health health = go.GetComponent<Health>();
        if (health != null) {
            health.Damage(1000000);
        }
    }

    void OnCollisionEnter(Collision collision) {
        _kill(collision.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody != null){
            _kill(other.attachedRigidbody.gameObject);
        }
    }
}
