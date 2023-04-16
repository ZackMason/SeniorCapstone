using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSpin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 500 * Time.deltaTime, 0f, Space.Self);
    }
}
