using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTankBrain : MonoBehaviour, ITankBrain
{
    public Vector2 GetDriveInput() {
        return new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );
    }

    public Vector2 GetTurretInput() {
        return new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );
    }

    public bool WantToZoom() {
        return Input.GetAxis("Fire2") > 0.0f;
    }
    
    public bool WantToFire() {
        return Input.GetAxis("Fire1") > 0.0f;
    }
}
