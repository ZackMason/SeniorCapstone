using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAIController
{
    public Vector2 GetMovement();
    public Vector3 GetArmTarget();
        
    // note(zack): only used by player controller
    // not needed if we are using first person
    public Vector2 GetCameraMovement();
}
