using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerTankController : MonoBehaviour
{

    private ITankBrain _brain;
    private Rigidbody _tankRigidbody;
    private Rigidbody _sphereRigidbody;

    public GameObject TankHead;
    public GameObject TankBody;
    public GameObject Sphere;


    void Start()
    {
        Debug.Assert(Sphere != null);
        Debug.Assert(TankBody != null);
        Debug.Assert(TankHead != null);

        _brain = GetComponent<ITankBrain>();
        _sphereRigidbody = Sphere.GetComponent<Rigidbody>();
        _tankRigidbody = GetComponent<Rigidbody>();

        Debug.Assert(_brain != null);
    }

    void Update()
    {
        Vector2 TurretInput = _brain.GetTurretInput();
        Vector2 DriveInput = _brain.GetDriveInput();

        TankHead.transform.Rotate(0.0f, TurretInput.x, 0.0f, Space.World);
        TankHead.transform.Rotate(TurretInput.y, 0.0f, 0.0f, Space.Self);

        Vector3 TurretForward = -TankHead.transform.forward;
        TurretForward.y = 0.0f;

        _sphereRigidbody.AddForce(TurretForward * 1.0f * DriveInput.y + _tankRigidbody.velocity * 0.5f);
        _tankRigidbody.velocity = new Vector3();
        _tankRigidbody.AddTorque(Vector3.up * DriveInput.x * 0.1f);
        _tankRigidbody.position = _sphereRigidbody.position;
    }
}
