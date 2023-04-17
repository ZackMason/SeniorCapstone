using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TentacleLink {
    public TentacleLink Next { get; set; }
    public TentacleLink Prev { get; private set; }

    public float StartDistance { get; private set; }
    public Vector3 LastPosition { get; private set; }
    public Transform Bone;

    public TentacleLink(Transform bone, TentacleLink prev = null) {
        Bone = bone;
        LastPosition = Bone.position;
        if ((Prev = prev) != null) {
            StartDistance = (Prev.Bone.position - LastPosition).magnitude;
        }
    }

    public void ConstrainDistance() {
        if (Prev == null) {
            return;
        }
        Vector3 dir = -((Prev.Bone.position) - Bone.position).normalized;
        
        Bone.position = Prev.Bone.position + dir * StartDistance;
    }

    private Vector3 _springForce(float dt, float force){
        if (Prev == null) {
            return Vector3.zero;
        }
        float disp = StartDistance * 0.5f;
        Vector3 delta = ((Prev.Bone.position) - Bone.position);
        float prevDist = delta.magnitude;
        float springDelta = prevDist - disp;
        return delta.normalized * springDelta * force * dt;
    }

    public void Integrate(float dt) {
        var boneT = Bone;
        Vector3 verletVelocity = boneT.position - LastPosition;

        if (Prev != null) {
            // verletVelocity += _springForce(dt, 1f);
        }

        // Todo(Zack): Apply Spring Force

        boneT.position += verletVelocity * dt;

        LastPosition = boneT.position;
    }
}

public class TentacleAnimator : MonoBehaviour
{
    [SerializeField] private TentacleLink _baseTentacle;
    [SerializeField] private TentacleLink _tipTentacle;

    void Start() {
        _tipTentacle = _baseTentacle = new TentacleLink(transform.Find("Armature/Bone"));
        while (_tipTentacle.Bone.childCount > 0) {
            _tipTentacle = _tipTentacle.Next = new TentacleLink(_tipTentacle.Bone.GetChild(0), _tipTentacle);
        }
    }   

    void FixedUpdate() {
        TentacleLink curTentacle = _baseTentacle;
        while(curTentacle != null) {
            curTentacle.Integrate(Time.fixedDeltaTime);
            curTentacle = curTentacle.Next;
        }
        curTentacle = _baseTentacle;
        while(curTentacle != null) {
            curTentacle.ConstrainDistance();
            curTentacle = curTentacle.Next;
        }
    }














}