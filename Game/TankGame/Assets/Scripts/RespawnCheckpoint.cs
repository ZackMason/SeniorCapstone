using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody?.tag == "Player") {
            RespawnManager.Instance.SetRespawnTransform(other.transform);
            Destroy(gameObject);
        }
    }

}
