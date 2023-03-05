using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public ITankBrain  _brain;
    private IWeapon     _weapon;
    public GameObject   TurretBody;
    public GameObject   TurretBarrel;

    void Awake()
    {
        Debug.Assert(TurretBody != null);
        Debug.Assert(TurretBarrel != null);

        if (_brain == null) {
            _brain = GetComponent<ITankBrain>();
        }
        _weapon = GetComponentInChildren<IWeapon>();

        Debug.Assert(_brain != null);
        Debug.Assert(_weapon != null);
    }

    void FixedUpdate()
    {
        Vector2 TurretInput = _brain.GetTurretInput() * Time.fixedDeltaTime * 100.0f;

        if (Cursor.lockState == CursorLockMode.Locked) {
            TurretBarrel.transform.Rotate(TurretInput.y, 0.0f, 0.0f, Space.Self);
            TurretBody.transform.Rotate(0.0f, TurretInput.x, 0.0f, Space.Self);
        }

        if (_brain.WantToFire() && _weapon != null) {
            _weapon.Fire();
        }
    }
}
