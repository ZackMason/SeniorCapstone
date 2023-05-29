using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class SnakeTargetFinder
{
    public float ArenaRadius = 100f;
    public Vector3 Exit = Vector3.zero;
    public Vector3 Target = Vector3.zero;

    [SerializeField] private Transform _arenaCenter;

    private Vector3 _calculateExitPoint(Vector3 target) {
        var player = _getPlayer();
        Debug.Assert(player); // TODO(Zack): Might hit if player dies, find random point if we do

        Vector3 toTarget = (target - _arenaCenter.position);
        toTarget.y = 0f;
        Vector3 exit = _arenaCenter.position - toTarget.normalized * ArenaRadius;

        return exit;
    }

    private GameObject _getPlayer() => RespawnManager.Instance.Player;

    public void CenterAttackTarget() {
        Exit = UnityEngine.Random.insideUnitSphere * ArenaRadius * 0.5f;
        Target = UnityEngine.Random.insideUnitSphere * ArenaRadius * 0.5f;
        Target.y = -80f; 
        Exit.y = 0f;
    }

    public void SelectTarget(Vector3 target) {
        if (target == Vector3.zero) {
            CenterAttackTarget();
        } else {
            Exit = _calculateExitPoint(target) - _arenaCenter.position;
            Target = target - _arenaCenter.position;
        }
    }
}
