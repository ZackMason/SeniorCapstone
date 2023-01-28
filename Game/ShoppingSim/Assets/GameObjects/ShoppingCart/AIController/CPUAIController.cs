using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUAIController : MonoBehaviour, IAIController
{
    public Vector2 GetMovement()
    {
        Vector2 Movement = new Vector2();
        
        // todo(zack): Implement ai here
        
        return Movement;
    }

    public Vector3 GetArmTarget()
    {
        return new Vector3();
    }

    public Vector2 GetCameraMovement()
    {
        return new Vector2();
    }

    public bool IsGrabbing()
    {
        return false;
    }
}
