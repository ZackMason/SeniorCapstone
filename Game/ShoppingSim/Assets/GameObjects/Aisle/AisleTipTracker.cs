using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AisleTipTracker : MonoBehaviour
{
    public float DistanceThreshold;

    private Vector3 _startPosition;

    void Start() {
        _startPosition = this.gameObject.transform.position;
    }

    void Update() {
        float StartDistance = Vector3.Distance(_startPosition, this.gameObject.transform.position);

        if (StartDistance > DistanceThreshold ||
            Vector3.Dot(this.gameObject.transform.up, new Vector3(0, 1, 0)) < 0.0f) { 
            Debug.Log("Aisle Has Tipped");

            ScoreManager.Instance.OnShelfDamaged();

            // destroy self
            Destroy(GetComponent<AisleTipTracker>());      
        }
    }
}
