using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBarrier : MonoBehaviour
{
    public string KeyName;

    // NOTE(Zack): Will be called by KeyManager
    public void OpenBarrier() {
        Destroy(this.gameObject);
    }
}
