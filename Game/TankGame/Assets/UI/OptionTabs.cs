using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionTabs : MonoBehaviour
{
    [SerializeField] private Transform _optionsPanel;
    private void _closeTabs() {
        foreach(Transform child in _optionsPanel) {
            child.gameObject.SetActive(false);
        }
    }

    void Start() {
        foreach(Transform child in transform) {
            var button = child.gameObject.GetComponent<MenuTabButton>();
            button.CloseTabs += _closeTabs;
        }
    }
}
