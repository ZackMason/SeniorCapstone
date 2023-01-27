using UnityEngine;

public class TempMovementController : MonoBehaviour {
    [SerializeField] private TempPlayerController tempPlayerController;
    [SerializeField] private float acceleration, rotationalAcceleration;

    private Rigidbody targetRigidbody;

    private void Start() {
        targetRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector2 move = tempPlayerController.GetMove();

        targetRigidbody.AddForce(move.y * transform.forward * acceleration * targetRigidbody.mass);
        targetRigidbody.AddTorque(move.x * transform.up * rotationalAcceleration * targetRigidbody.mass);
        // if (Input.GetKey("w")) {
        //     targetRigidbody.AddForce(transform.forward * acceleration * targetRigidbody.mass);
        // }
        // if (Input.GetKey("a")) {
        //     targetRigidbody.AddTorque(-transform.up * rotationalAcceleration * targetRigidbody.mass);
        // }
        // if (Input.GetKey("s")) {
        //     targetRigidbody.AddForce(-transform.forward * acceleration * targetRigidbody.mass);
        // }
        // if (Input.GetKey("d")) {
        //     targetRigidbody.AddTorque(transform.up * rotationalAcceleration * targetRigidbody.mass);
        // }
    }
}
