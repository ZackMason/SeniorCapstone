using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject DestroyedPrefab;

    [Range(0, 10000)]
    public float RamResistance;

    public void Destruct() {
        if (DestroyedPrefab) {
            var savedScale = transform.localScale;
            var newObject = Instantiate(DestroyedPrefab, transform.position, transform.rotation);
            newObject.transform.localScale = savedScale;
        }
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision col)
    {
        Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();

        if (rb != null) {
            float kineticEnergy = col.relativeVelocity.magnitude * col.relativeVelocity.magnitude * rb.mass * 0.5f;
            // Debug.Log("rb hit: " + kineticEnergy);

            if (kineticEnergy >= RamResistance) {
                Destruct();

                ExplosionManager.Instance.SpawnExplosion(col.contacts[0].point, 2.0f, kineticEnergy * 0.05f, 1.0f);
            }
        }
        
    }
}
