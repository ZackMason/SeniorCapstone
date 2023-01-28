using UnityEngine;

public class TempCameraController : MonoBehaviour {
    [SerializeField] private PlayerAIController playerAIController;
    [SerializeField] private float edgeProportion, rotationSpeed1, rotationSpeed2;

    private Vector2 defaultEulerAngles;
    private bool returnToCenter = false;

    private bool xCoordCentered = false;
    private bool yCoordCentered = false;

    private bool rightMouseButtonDown = false;

    private void Start() {
        defaultEulerAngles = transform.localEulerAngles;
    }

    private void Update() {
        if (Input.GetKey("r")) {
            returnToCenter = true;
        }

        // rightMouseButtonDown = Input.GetMouseButtonDown(1);
        rightMouseButtonDown = Input.GetKey("e");

        Vector2 newEulerAngles = transform.localEulerAngles;

        if (playerAIController.tempMode == 2) {
            if (rightMouseButtonDown) {
                Cursor.lockState = CursorLockMode.None;

                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
            
            newEulerAngles.x -= Input.GetAxis("Mouse Y") * rotationSpeed2 * Time.deltaTime;
            newEulerAngles.y += Input.GetAxis("Mouse X") * rotationSpeed2 * Time.deltaTime;

            newEulerAngles.x = Mathf.Clamp((newEulerAngles.x + 180.0f) % 360.0f - 180.0f, -89.0f, 89.0f);

            transform.localEulerAngles = newEulerAngles;
            
            return;
        }

        Cursor.lockState = CursorLockMode.None;

        if (playerAIController.tempMode == 0) {
            return;
        }

        if (returnToCenter) {
            if (transform.localEulerAngles.x < 180.0f) {
                newEulerAngles.x -= rotationSpeed1 * Time.deltaTime;

                if (newEulerAngles.x <= 0.0f) {
                    xCoordCentered = true;
                }
            } else {
                newEulerAngles.x += rotationSpeed1 * Time.deltaTime;

                if (newEulerAngles.x >= 360.0f) {
                    xCoordCentered = true;
                }
            }
            if (transform.localEulerAngles.y < 180.0f) {
                newEulerAngles.y -= rotationSpeed1 * Time.deltaTime;

                if (newEulerAngles.y <= 0.0f) {
                    yCoordCentered = true;
                }
            } else {
                newEulerAngles.y += rotationSpeed1 * Time.deltaTime;

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
            newEulerAngles.y -= rotationSpeed1 * Time.deltaTime;
        }

        if (Input.mousePosition.x >= Screen.width - Mathf.Min(Screen.width, Screen.height) * edgeProportion) {
            newEulerAngles.y += rotationSpeed1 * Time.deltaTime;
        }

        if (Input.mousePosition.y <= Mathf.Min(Screen.width, Screen.height) * edgeProportion) {
            newEulerAngles.x += rotationSpeed1 * Time.deltaTime;
        }

        if (Input.mousePosition.y >= Screen.height - Mathf.Min(Screen.width, Screen.height) * edgeProportion) {
            newEulerAngles.x -= rotationSpeed1 * Time.deltaTime;
        }

        newEulerAngles.x = Mathf.Clamp((newEulerAngles.x + 180.0f) % 360.0f - 180.0f, -89.0f, 89.0f);

        transform.localEulerAngles = newEulerAngles;
        // transform.Rotate(newEulerAngles, Space.World);
        // transform.localRotation *= Quaternion.Euler(newEulerAngles);
    }
}
