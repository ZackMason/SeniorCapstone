using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        RespawnManager.Instance.SetRespawnPosition(other.transform.position);
        Destroy(gameObject);
    }

}
