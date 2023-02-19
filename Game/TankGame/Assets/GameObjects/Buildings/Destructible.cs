using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject DestroyedPrefab;

    [Range(0, 10)]
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
            // Debug.Log("rb hit: " + rb.velocity.magnitude);
            // TODO(Zack): Maybe add mass into this equation
            if (rb.velocity.magnitude >= RamResistance) {
                Destruct();
            }
        }
        
    }
}
