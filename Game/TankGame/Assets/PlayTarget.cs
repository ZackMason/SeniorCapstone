using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTarget : MonoBehaviour
{
    void OnDestroy()
    {
        LevelManager.Instance.LoadScene("SampleScene");
    }
}
