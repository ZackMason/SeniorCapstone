using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDelete : MonoBehaviour
{
    public float LifeTime;
    
    void FixedUpdate()
    {
        LifeTime -= Time.fixedDeltaTime;
        if (LifeTime < 0.0f) {
            Destroy(this.gameObject);
            Timer.score += 1;
        }        
    }
}
