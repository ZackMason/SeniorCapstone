using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;

    public static TimeManager Instance { get { return _instance; } }

    [Range(60.0f, 60.0f*5.0f)]
    public float TimeLimit;
    
    
}
