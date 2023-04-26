using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && 
            !LevelManager.Instance.IsShowingOptions()) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Escape) ||
            (Gamepad.current?.selectButton.wasPressedThisFrame ?? false)) {
            if (LevelManager.Instance.IsShowingOptions()) {
                LevelManager.Instance.ShowOptions(false);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
            } else {
                LevelManager.Instance.ShowOptions(true);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
            }
        }
    }
}
