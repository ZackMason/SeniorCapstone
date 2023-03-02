using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note(Zack): This script attachs TimedDelete 
// to a random percentage of child game objects
public class DeletePerc : MonoBehaviour
{
    [Range(0.0f, 60.0f)]
    public float LifeTime;

    [Range(0.0f, 1.0f)]
    public float Perc;

    void Start()
    {
        foreach(Transform child in gameObject.transform) {
            if (Random.Range(0.0f, 1.0f) < Perc) {
                TimedDelete timer = child.gameObject.AddComponent(typeof(TimedDelete)) as TimedDelete;
                timer.LifeTime = LifeTime;
            }
        }
    }
}
