using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankExplode : MonoBehaviour
{
    void Update()
    {
        ExplosionManager.Instance.SpawnExplosion(
            transform.position + Vector3.down * 1.0f,
            5.0f,
            100000.0f,
            0.0f
        );
        Destroy(this);
    }
}
