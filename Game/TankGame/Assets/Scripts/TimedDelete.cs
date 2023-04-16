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
        while (Time.time < end && transform != null) {
            float t = (Time.time-start) / (end-start);
            transform.localScale = new Vector3(1,1,1) * ScaleCurve.Evaluate(t);
            yield return null;
        }
    }

    private IEnumerator _delete() {
        yield return new WaitForSeconds(LifeTime);
        Destroy(this.gameObject);
    }
}
