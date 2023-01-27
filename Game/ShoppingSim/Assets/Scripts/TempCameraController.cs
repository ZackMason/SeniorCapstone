using UnityEngine;

public class TempCameraController : MonoBehaviour {
    [SerializeField] private TempPlayerController tempPlayerController;
    [SerializeField] private float edgeProportion, rotationSpeed;

    private Vector2 defaultEulerAngles;
    private bool returnToCenter = false;

    private bool xCoordCentered = false;
    private bool yCoordCentered = false;

    private void Start() {
        defaultEulerAngles = transform.localEulerAngles;
    }

    private void FixedUpdate() {
        if (tempPlayerController.tempMode == 0) {
            return;
        }

        if (Input.GetKey("r")) {
            returnToCenter = true;
        }

        Vector2 newEulerAngles = transform.localEulerAngles;
        // Vector2 newEulerAngles = Vector2.zero;

        if (returnToCenter) {
            if (transform.localEulerAngles.x < 180.0f) {
                newEulerAngles.x -= rotationSpeed * Time.fixedDeltaTime;

                if (newEulerAngles.x <= 0.0f) {
                    xCoordCentered = true;
                }
            } else {
                newEulerAngles.x += rotationSpeed * Time.fixedDeltaTime;

                if (newEulerAngles.x >= 360.0f) {
                    xCoordCentered = true;
                }
            }
            if (transform.localEulerAngles.y < 180.0f) {
                newEulerAngles.y -= rotationSpeed * Time.fixedDeltaTime;

                if (newEulerAngles.y <= 0.0f) {
                    yCoordCentered = true;
                }
            } else {
                newEulerAngles.y += rotationSpeed * Time.fixedDeltaTime;

                if (newEulerAngles.y >= 360.0f) {
                    yCoordCentered = true;
                }
            }

            if (xCoordCentered) {
                newEulerAngles.x = defaultEulerAngles.x;
            }
            if (yCoordCentered) {
                newEulerAngles.y = defaultEulerAngles.y;
            }

            if (Mathf.Approximately(newEulerAngles.x % 360.0f, defaultEulerAngles.x % 360.0f) && Mathf.Approximately(newEulerAngles.y % 360.0f, defaultEulerAngles.y % 360.0f)) {
                returnToCenter = false;
                xCoordCentered = false;
                yCoordCentered = false;

                transform.localEulerAngles = defaultEulerAngles;

                return;
            }
            
            transform.localEulerAngles = newEulerAngles;

            return;
        }

        if (Input.mousePosition.x <= Mathf.Min(Screen.width, Screen.height) * edgeProportion) {
            newEulerAngles.y -= rotationSpeed * Time.fixedDeltaTime;
        }

        if (Input.mousePosition.x >= Screen.width - Mathf.Min(Screen.width, Screen.height) * edgeProportion) {
            newEulerAngles.y += rotationSpeed * Time.fixedDeltaTime;
        }

        if (Input.mousePosition.y <= Mathf.Min(Screen.width, Screen.height) * edgeProportion) {
            newEulerAngles.x += rotationSpeed * Time.fixedDeltaTime;
        }

        if (Input.mousePosition.y >= Screen.height - Mathf.Min(Screen.width, Screen.height) * edgeProportion) {
            newEulerAngles.x -= rotationSpeed * Time.fixedDeltaTime;
        }

        newEulerAngles.x = Mathf.Clamp((newEulerAngles.x + 180.0f) % 360.0f - 180.0f, -89.0f, 89.0f);

        transform.localEulerAngles = newEulerAngles;
        // transform.Rotate(newEulerAngles, Space.World);
        // transform.localRotation *= Quaternion.Euler(newEulerAngles);
    }
}
