using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public Vector3 Target;
    private Faction _faction;

    public bool NeedsVision = false;
    public float SearchTime = 1;
    private float _searchTimer;

    void Start()
    {
        _faction = GetComponent<Faction>();
    }

    void Update()
    {
        if ((_searchTimer -= Time.deltaTime) < 0.0f) {
            _searchTimer = SearchTime;

            float closest = float.MaxValue;
            Faction[] factions = FindObjectsOfType(typeof(Faction)) as Faction[];
            foreach(Faction faction in factions) {
                if (faction.ID != _faction.ID) {
                    float dist = Vector3.Magnitude(faction.gameObject.transform.position - transform.position);
                    float tooClose = 3.0f;
                    if (dist < closest && dist > tooClose) {
                        if (NeedsVision) {
                            int layerMask = ~0;
                            Vector3 rayOrigin = transform.position;
                            Vector3 rayDirection = Vector3.Normalize(faction.gameObject.transform.position - transform.position);
                            RaycastHit hit;

                            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, layerMask)) {
                                // Note(Zack): This looks dumb, because it is
                                if (hit.rigidbody) {
                                    if (hit.rigidbody.gameObject != faction.gameObject) {
                                        continue;
                                    }
                                } else if (hit.collider) {
                                    if (hit.collider.gameObject != faction.gameObject) {
                                        continue;
                                    }
                                } else {
                                    continue;
                                }
                            } else {
                                Debug.Log("Target finder cant see anything?");
                            }
                        }
                        closest = dist;
                        Target = faction.gameObject.transform.position; 
                    }
                }
            }
        }
    }
}
