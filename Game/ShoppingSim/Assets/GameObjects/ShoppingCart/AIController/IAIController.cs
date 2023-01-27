using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAIController
{
    public Vector2 GetMovement();
    // note(zack): 3d world position that the arm should move to
    public Vector3 GetArmTarget();
        
    // note(zack): only used by player controller
    // not needed if we are using first person
    public Vector2 GetCameraMovement();

    public bool IsGrabbing();
}
