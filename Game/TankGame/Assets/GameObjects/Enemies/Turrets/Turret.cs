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
        Vector2 currentTurretInput = new Vector2(TurretBarrel.transform.rotation.eulerAngles.x, TurretBody.transform.rotation.eulerAngles.y);
        Vector2 TurretInput = _brain.GetTurretInput() * Time.fixedDeltaTime * 50.0f;
        currentTurretInput = Vector2.Lerp(currentTurretInput, TurretInput, 50 * Time.deltaTime);

        TurretBarrel.transform.Rotate(currentTurretInput.y, 0.0f, 0.0f, Space.Self);
        TurretBody.transform.Rotate(0.0f, currentTurretInput.x, 0.0f, Space.Self);

        if (_brain.WantToFire() && _weapon != null) {
            _weapon.Fire();
        }
    }
}
