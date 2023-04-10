using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    public List<GameObject> Units { get; private set; }
    
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);
        }
        else 
        { 
            Instance = this;
            Units = new List<GameObject>();
        }
    }

    public void AddUnit(GameObject unit) {
        Units.Add(unit);
    }


}
