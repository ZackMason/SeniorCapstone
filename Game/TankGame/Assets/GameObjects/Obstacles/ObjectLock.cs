using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLock : MonoBehaviour
{
    public GameObject Lock;
    void Update() {
        if (Lock == null) {
            Destroy(this.gameObject);
        }
    }
}
