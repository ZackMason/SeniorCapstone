using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class SnakeController : MonoBehaviour
{
    [SerializeField] private CinemachineDollyCart _dollyCart;
    [SerializeField] private float _moveSpeed;
    private float _pathPosition = 0f;

    [SerializeField] SnakeTargetFinder _targetFinder = new SnakeTargetFinder();

    private void _setCartPosition(float p) => _dollyCart.m_Position = p;
    private CinemachineSmoothPath _getPath() => _dollyCart.m_Path as CinemachineSmoothPath;
    
    void Update()
    {
        _pathPosition += Time.deltaTime / _moveSpeed;
        if (_pathPosition >= 1.1f) {
            _pathPosition = 0f;

            if (UnityEngine.Random.value < 0.5f) {
                _jumpTowardPlayer();
            } else {
                _groundAttack();
            }

        }
        _setCartPosition(_pathPosition);
    }

    private void _jumpCenter() {
        _targetFinder.SelectTarget(Vector3.zero);
        float height = 25f;
        Vector3 sphereScale = new Vector3(1f, 0.2f, 1f);
        var path = _getPath();
        
        path.m_Waypoints[0].position = _targetFinder.Exit + Vector3.down * 10f;
        path.m_Waypoints[1].position = _targetFinder.Exit * 0.5f + _targetFinder.Target * 0.5f;
        path.m_Waypoints[2].position = _targetFinder.Exit * 0.5f + _targetFinder.Target * 0.5f;
        path.m_Waypoints[1].position.y = height;
        path.m_Waypoints[2].position.y = height;
        path.m_Waypoints[1].position += Vector3.Scale(sphereScale, UnityEngine.Random.insideUnitSphere) * 10f;
        path.m_Waypoints[2].position += Vector3.Scale(sphereScale, UnityEngine.Random.insideUnitSphere) * 10f;
        path.m_Waypoints[3].position = _targetFinder.Target;
            
        path.InvalidateDistanceCache();
    }

    private void _jumpTowardPlayer() {
        _jumpTowardTarget(RespawnManager.Instance.Player.transform.position);
    }

    private void _jumpTowardTarget(Vector3 target) {
        _targetFinder.SelectTarget(target);
        float height = 25f;
        var path = _getPath();
        
        path.m_Waypoints[0].position = _targetFinder.Exit + Vector3.down * 10f;
        path.m_Waypoints[1].position = _targetFinder.Exit * 0.5f + _targetFinder.Target * 0.5f;
        path.m_Waypoints[1].position.y = height;
        target = path.m_Waypoints[1].position;
        path.m_Waypoints[2].position = _targetFinder.Target;
        path.m_Waypoints[3].position =
            target + 
            (_targetFinder.Target - target).normalized * 
            ((_targetFinder.Target - target).magnitude *2f); // 80 is length of snakey
            
        path.InvalidateDistanceCache();
    }

    private void _groundAttack() {
        _targetFinder.SelectTarget(RespawnManager.Instance.Player.transform.position);
        float height = 5f;
        Vector3 sphereScale = new Vector3(1f, 0.00001f, 1f);
        var path = _getPath();
        
        path.m_Waypoints[0].position = _targetFinder.Exit + Vector3.down * 10f;
        path.m_Waypoints[1].position = _targetFinder.Exit * 0.5f + _targetFinder.Target * 0.5f;
        path.m_Waypoints[2].position = _targetFinder.Exit * 0.5f + _targetFinder.Target * 0.5f;
        path.m_Waypoints[1].position.y = height;
        path.m_Waypoints[2].position.y = height;
        path.m_Waypoints[1].position += Vector3.Scale(sphereScale, UnityEngine.Random.insideUnitSphere) * 30f;
        path.m_Waypoints[2].position += Vector3.Scale(sphereScale, UnityEngine.Random.insideUnitSphere) * 30f;
        path.m_Waypoints[3].position = _targetFinder.Target;
        // Vector3 target = path.m_Waypoints[2].position;
        // path.m_Waypoints[3].position =
        //     target + 
        //     (_targetFinder.Target - target).normalized * 
        //     ((_targetFinder.Target - target).magnitude *2f); // 80 is length of snakey
            
        path.InvalidateDistanceCache();
    }
}
