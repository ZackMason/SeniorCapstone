using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCamera : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotSpeed = 1f;

    private Vector2 _driveInput;
    private Vector2 _turnInput;

    void OnDrive(InputValue value)
    {
        _driveInput = value.Get<Vector2>();
    }

    void OnTurnTurret(InputValue value)
    {
        _turnInput = value.Get<Vector2>();
    }

    private void Update()
    {
        // Calculate the movement vector based on the input
        Vector3 movement = new Vector3(_driveInput.x, 0f, _driveInput.y) * _moveSpeed * Time.deltaTime;

        // Move the camera along the calculated movement vector
        transform.Translate(movement);

        // Calculate the rotation angles based on the input
        float rotationY = _turnInput.x * _rotSpeed * Time.deltaTime;
        float rotationX = -_turnInput.y * _rotSpeed * Time.deltaTime;

        // Rotate the camera and turret
        transform.Rotate(Vector3.right, rotationX);
        transform.Rotate(Vector3.up, rotationY);

        // Restrict camera roll rotation
        float currentZRotation = transform.eulerAngles.z;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, currentZRotation);
    }
}
