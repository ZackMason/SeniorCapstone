using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject DestroyedPrefab;

    [Range(0, 10000)]
    public float RamResistance;

    public void Destruct() {
        Debug.Assert(DestroyedPrefab != null);

        Instantiate(DestroyedPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision col)
    {
        Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();

        if (rb != null) {
            // TODO(Zack): Maybe add mass into this equation
            float kineticEnergy = rb.velocity.magnitude * rb.velocity.magnitude * rb.mass * 0.5f;
            // Debug.Log("rb hit: " + kineticEnergy);

            if (kineticEnergy >= RamResistance) {
                Destruct();

                ExplosionManager.Instance.SpawnExplosion(col.contacts[0].point, 2.0f, kineticEnergy * 0.05f);
            }
        }
        
    }
}
