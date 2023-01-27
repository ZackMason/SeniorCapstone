using UnityEngine;

public class TempMovementController : MonoBehaviour {
    [SerializeField] private float acceleration, rotationalAcceleration;

    private IAIController AiController;
    private Rigidbody targetRigidbody;

    private void Start() {
        AiController = GetComponent<PlayerAIController>();

        if (AiController == null) {
            AiController = GetComponent<CPUAIController>();
        }

        Debug.Assert(AiController != null);
        targetRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector2 Movement = AiController.GetMovement();

        targetRigidbody.AddForce(transform.forward * acceleration * targetRigidbody.mass * Movement.y);
        targetRigidbody.AddForce(transform.right * acceleration * targetRigidbody.mass * Movement.x);
    }
}
