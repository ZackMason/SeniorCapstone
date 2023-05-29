using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTarget : MonoBehaviour {
    [SerializeField] private string _sceneName = "SampleScene";

    void Start() {
        GetComponent<Health>().OnDeath += _onDeath;
    }

    private void _onDeath() =>
        LevelManager.Instance.LoadScene(_sceneName);
}
