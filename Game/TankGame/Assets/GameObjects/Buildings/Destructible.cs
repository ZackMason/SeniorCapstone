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
        Rigidbody otherBody = col.gameObject.GetComponent<Rigidbody>();
        Rigidbody body = GetComponent<Rigidbody>();
        float otherKineticEnergy = 0f;
        float selfKineticEnergy = 0f;

        if (body == null) {
            body = GetComponentInParent<Rigidbody>();
        }
        
        if (body != null) {
            selfKineticEnergy = body.velocity.magnitude * body.velocity.magnitude * body.mass * 0.5f;
        }
        if (otherBody != null) {
            otherKineticEnergy = otherBody.velocity.magnitude * otherBody.velocity.magnitude * otherBody.mass * 0.5f;
        }

        float totalKineticEnergy = (otherKineticEnergy + selfKineticEnergy);
        if (totalKineticEnergy >= RamResistance) {
            Destruct();

            ExplosionManager.Instance.SpawnExplosion(col.contacts[0].point, 2.0f, totalKineticEnergy * 0.05f, 1.0f);
        }
    }
}
