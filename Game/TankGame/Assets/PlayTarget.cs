using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTarget : MonoBehaviour {
    [SerializeField] private string _sceneName = "SampleScene";
    void OnDestroy() =>
        LevelManager.Instance.LoadScene(_sceneName);
}
