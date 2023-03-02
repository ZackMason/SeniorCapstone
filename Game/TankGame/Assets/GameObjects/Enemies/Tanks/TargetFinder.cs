using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public EnemyTankBrain Brain;
    private Faction _faction;


    void Start()
    {
        _faction = GetComponent<Faction>();
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
                    Brain.Target = faction.gameObject.transform.position; 
                }
            }
        }
    }
}
