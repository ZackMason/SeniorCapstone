using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class TargetInfo {
    public GameObject target;
    public Transform transform;
}

public class SpinTargets : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private GameObject _targetPrefab;

    [SerializeField] private List<GameObject> _targets = new List<GameObject>();
    [SerializeField] private List<Vector3> _positions = new List<Vector3>();

    void Start() {
        foreach(Transform child in transform) {
            _targets.Add(child.gameObject);
            _positions.Add(child.transform.localPosition);
        }
    }

    void Update()
    {
        transform.eulerAngles = Vector3.right * Time.time * _rotateSpeed;

        if (_targets.All(e => e == null)) {
            for (int i = 0; i < _targets.Count; i++) {
                _targets[i] = Instantiate(_targetPrefab);
                _targets[i].transform.SetParent(transform);
                _targets[i].transform.localPosition = _positions[i];
            }
        }
    }
}
