using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAIController : MonoBehaviour, IAIController
{
    [SerializeField] private int numOfLayers;
    [SerializeField] private string grabbedItemLayer, raycastColliderLayer;
    [SerializeField] private Collider raycastCollider;
    [SerializeField] private float raycastDistance;

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
        RaycastHit hitInfo;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (tempMode == 0) {
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ((int) (Mathf.Pow(2.0f, numOfLayers)) - 1) ^ LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer), grabbedItemLayer, raycastColliderLayer));
        } else {
            raycastCollider.Raycast(new Ray(ray.GetPoint(raycastDistance), -ray.direction), out hitInfo, raycastDistance);
        }

        return hitInfo.point;
    }

    public Vector2 GetCameraMovement()
    {
        return new Vector2();
    }
}
