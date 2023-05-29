using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankBrain
{
    public Vector2 GetDriveInput();
    public Vector2 GetTurretInput();
    public bool WantToFire();
    public float GetBoost();
    public float GetTurbo();
    public bool GetAirbrake();

    // NOTE(ZACK): only used by player
    public bool WantToZoom();
    public bool WantToSwitchMode();
}
