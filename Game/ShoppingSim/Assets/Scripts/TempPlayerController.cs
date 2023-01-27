using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayerController : MonoBehaviour {
    public int tempMode;

    private PlayerInput playerInput;
    private InputAction moveAction;

    private Vector2 move;

    private void Start() {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.currentActionMap.FindAction("move");
    }

    public Vector2 GetMove() {
        return moveAction.ReadValue<Vector2>();
    }
}
