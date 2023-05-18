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
            _targetFinder.SelectTarget(RespawnManager.Instance.Player.transform.position);
            float height = _getPath().m_Waypoints[1].position.y;
            
            _getPath().m_Waypoints[0].position = _targetFinder.Exit + Vector3.down * 10f;
            _getPath().m_Waypoints[1].position = _targetFinder.Exit * 0.5f + _targetFinder.Target * 0.5f;
            _getPath().m_Waypoints[1].position.y = height;
            Vector3 target = _getPath().m_Waypoints[1].position;
            _getPath().m_Waypoints[2].position = _targetFinder.Target;
            _getPath().m_Waypoints[3].position =
                target + 
                (_targetFinder.Target - target).normalized * 
                ((_targetFinder.Target - target).magnitude *2f); // 80 is length of snakey
                
            _getPath().InvalidateDistanceCache();
        }
        _setCartPosition(_pathPosition);
    }
}
