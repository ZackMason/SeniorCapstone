using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    private Faction _faction;

    public Vector3 Target;

    void Start()
    {
        _faction = GetComponent<Faction>();
        Debug.Assert(_faction);
    }

    void Update()
    {
        float closest = float.MaxValue;
        Faction[] factions = FindObjectsOfType(typeof(Faction)) as Faction[];
        foreach(Faction faction in factions) {
            if (faction.ID != _faction.ID) {
                float dist = Vector3.Magnitude(faction.gameObject.transform.position - transform.position);
                float tooClose = 3.0f;
                if (dist < closest && dist > tooClose) {
                    closest = dist;
                    Target = faction.gameObject.transform.position; 
                }
            }
        }
    }
}
