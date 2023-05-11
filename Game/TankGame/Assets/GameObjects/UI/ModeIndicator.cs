using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _on;
    [SerializeField] private GameObject _off;

    private bool _state = false;

    public bool SetState(bool s) {
        bool result = _state != s;
        _state = s;
        _update();
        return result;
    }

    public void Toggle() {
        _state = !_state;
        _update();
    }
    
    private void _update() {
        _on.SetActive(_state);
        _off.SetActive(!_state);
    }
}
