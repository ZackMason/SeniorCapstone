using UnityEngine;
using UnityEngine.InputSystem;

    [RequireComponent( typeof(Camera) )]
public class FlyCamera : MonoBehaviour {
	public float acceleration = 50; // how fast you accelerate
	public float accSprintMultiplier = 4; // how much faster you go when "sprinting"
	public float lookSensitivity = 1; // mouse look sensitivity
	public float dampingCoefficient = 5; // how quickly you break to a halt after you stop your input
	public bool focusOnEnable = true; // whether or not to focus and lock cursor immediately on enable

	Vector3 velocity; // current velocity
    Vector2 _driveInput;
    Vector2 _turnInput;

	static bool Focused {
		get => Cursor.lockState == CursorLockMode.Locked;
		set {
			Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = value == false;
		}
	}
    void OnDrive(InputValue value)
    {
        _driveInput = value.Get<Vector2>();
    }

    void OnTurnTurret(InputValue value)
    {
        _turnInput = value.Get<Vector2>();
    }

	void OnEnable() {
		if( focusOnEnable ) Focused = true;
	}

	void OnDisable() => Focused = false;

	void Update() {
		// Input
		if( Focused )
			UpdateInput();
		else if( Input.GetMouseButtonDown( 0 ) )
			Focused = true;

		// Physics
		velocity = Vector3.Lerp( velocity, Vector3.zero, dampingCoefficient * Time.deltaTime );
		transform.position += velocity * Time.deltaTime;
	}

	void UpdateInput() {
		// Position
		velocity += GetAccelerationVector() * Time.deltaTime;

		// Rotation
		Vector2 mouseDelta = lookSensitivity * _turnInput;
        mouseDelta.y *= -1f;
		Quaternion rotation = transform.rotation;
		Quaternion horiz = Quaternion.AngleAxis( mouseDelta.x, Vector3.up );
		Quaternion vert = Quaternion.AngleAxis( mouseDelta.y, Vector3.right );
		transform.rotation = horiz * rotation * vert;

		// Leave cursor lock
		if( Input.GetKeyDown( KeyCode.Escape ) )
			Focused = false;
	}

	Vector3 GetAccelerationVector() {
		Vector3 moveInput = Vector3.forward * _driveInput.y + Vector3.right * _driveInput.x;

		Vector3 direction = transform.TransformVector( moveInput.normalized );

		// if( Input.GetKey( KeyCode.LeftShift ) )
			// return direction * ( acceleration * accSprintMultiplier ); // "sprinting"
		return direction * acceleration; // "walking"
	}
}