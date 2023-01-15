using UnityEngine;

public class TempMovementController : MonoBehaviour {
    [SerializeField] private float speed;

    private Rigidbody targetRigidbody;

    private void Start() {
        targetRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (Input.GetKey("w")) {
            targetRigidbody.AddForce(transform.forward * speed * targetRigidbody.mass);
        }
        if (Input.GetKey("a")) {
            targetRigidbody.AddForce(-transform.right * speed * targetRigidbody.mass);
        }
        if (Input.GetKey("s")) {
            targetRigidbody.AddForce(-transform.forward * speed * targetRigidbody.mass);
        }
        if (Input.GetKey("d")) {
            targetRigidbody.AddForce(transform.right * speed * targetRigidbody.mass);
        }
    }
}
