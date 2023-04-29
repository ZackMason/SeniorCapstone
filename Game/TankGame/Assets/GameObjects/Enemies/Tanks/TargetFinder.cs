using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Faction))]
public class TargetFinder : MonoBehaviour
{
    public Vector3 Target;
    public Vector3 MoveTarget;
    public bool HasTarget = false;

    private Faction _faction;

    public bool NeedsVision = true;
    public float VisionRadius = 50.0f;
    public float SearchTime = 1;
    private float _searchTimer;

    void Start()
    {
        _faction = GetComponent<Faction>();
    }

    void Update()
    {
        Search(Time.deltaTime);
    }

    public void Search(float dt) {
        if ((_searchTimer -= dt) < 0.0f) {
            _searchTimer = SearchTime;

            float closest = float.MaxValue;
            // TODO(Zack): Fix this
            Faction[] factions = FindObjectsOfType(typeof(Faction)) as Faction[];
            foreach(Faction faction in factions) {
                if (faction.ID != _faction.ID) {
                    float dist = Vector3.Magnitude(faction.gameObject.transform.position - transform.position);
                    float tooClose = 3.0f;
                    if (dist < closest && dist > tooClose && dist < VisionRadius) {
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
                                continue;
                            }
                        }
                        closest = dist;
                        Target = faction.gameObject.transform.position; 
                        HasTarget = true;
                    }
                }
            }

            if (closest == float.MaxValue) {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-10.0f, 10.0f),
                    0.0f,
                    Random.Range(-10.0f, 10.0f)
                );
                Target = transform.position + randomOffset;
                HasTarget = false;
            }
        }
        MoveTarget = Target;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Target, .2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Target, .2f);
    }
}
