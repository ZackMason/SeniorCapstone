using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAIController : MonoBehaviour, IAIController
{
    public int tempMode;

    private PlayerInput playerInput;
    private InputAction moveAction;

    private void Start() {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.currentActionMap.FindAction("move");
    }

    public Vector2 GetMovement()
    {
        Vector2 Movement = moveAction.ReadValue<Vector2>();
        
        return Movement;
    }

    public Vector3 GetArmTarget()
    {
        return new Vector3();
    }

    public Vector2 GetCameraMovement()
    {
        return new Vector2();
    }
}
