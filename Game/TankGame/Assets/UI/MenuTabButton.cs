using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTabButton : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    public event Action CloseTabs;
    public void OnTab(){
        CloseTabs?.Invoke();
        _panel.SetActive(true);
    }
}
