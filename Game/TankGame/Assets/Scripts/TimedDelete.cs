using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TimedDelete : MonoBehaviour
{
    public float LifeTime;
    public AnimationCurve ScaleCurve;

    void Start() {
        StartCoroutine(_scale());
        StartCoroutine(_delete());
    }

    private IEnumerator _scale() {
        var start = Time.time;
        var end = Time.time + LifeTime - 0.1f;
        var startPos = transform.position;
        var rb = transform.GetComponent<Rigidbody>();

        while (Time.time < end && transform != null) {
            float t = (Time.time-start) / (end-start);
            float s = ScaleCurve.Evaluate(t);
            transform.localScale = new Vector3(1,1,1) * s;

            if (t > 0.5f){
                // TODO(Zack): @hardcoded 
                if (rb != null) {
                    if (rb.velocity.magnitude < 10f) {
                        // TODO(Zack): @hardcoded 
                        startPos = transform.position - new Vector3(0,-2.5f,0) * (1f-s);
                        Destroy(rb);
                    }
                } else {
                    // TODO(Zack): @hardcoded 
                    transform.position = startPos + new Vector3(0,-2.5f,0) * (1f-s);
                }
            }
            
            yield return null;
        }
    }

    private IEnumerator _delete() {
        yield return new WaitForSeconds(LifeTime);
        Destroy(this.gameObject);
    }
}
