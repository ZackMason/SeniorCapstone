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
        Faction[] factions = FindObjectsOfType(typeof(Faction)) as Faction[];
        foreach(Faction faction in factions) {
            if (faction.ID != _faction.ID) {
                Brain.Target = faction.gameObject.transform.position; 
            }
        }
    }
}
