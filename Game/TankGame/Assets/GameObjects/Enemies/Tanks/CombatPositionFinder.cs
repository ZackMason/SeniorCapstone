using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPositionFinder : TargetFinder
{
    [Range(1, 100)]
    public float MinDistance = 25.0f;
    
    void Update()
    {
        Search(Time.deltaTime);

        var target = Target;
        var position = transform.position;
        var halfway = (target + position) * 0.5f;
        var minPoint = (position - target).normalized * MinDistance + target;

        if (Vector3.Distance(target, halfway) < MinDistance) {
            Target = minPoint;
        } else {
            Target = halfway;
        }
    }
}
