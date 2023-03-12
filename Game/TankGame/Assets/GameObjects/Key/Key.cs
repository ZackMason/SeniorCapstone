using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public string Name;

    void Pickup() {
        KeyManager.Instance.PickupKey(Name);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody != null) {
            var faction = other.attachedRigidbody.gameObject.GetComponent<Faction>();
            if (faction != null && faction.ID == 0) {
                Pickup();
            }
        }
    }
}
