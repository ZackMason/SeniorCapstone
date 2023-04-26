using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour {
    public void OnPlay() {
        LevelManager.Instance.LoadScene("SampleScene");
        transform.parent.gameObject.SetActive(false);
    }
}
