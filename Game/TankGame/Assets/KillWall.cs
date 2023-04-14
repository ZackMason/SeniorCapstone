using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillWall : MonoBehaviour
{
    public void OnCollisionEnter(Collision col) {
        Destroy(col.rigidbody.gameObject);
    }
}
