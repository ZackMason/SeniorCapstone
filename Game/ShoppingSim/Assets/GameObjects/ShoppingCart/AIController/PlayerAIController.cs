using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIController : MonoBehaviour, IAIController
{
    public Vector2 GetMovement()
    {
        Vector2 Movement = new Vector2();
        
        if (Input.GetKey("w")) {
            Movement.y += 1.0f;
        }        
        if (Input.GetKey("s")) {
            Movement.y -= 1.0f;
        }        
        if (Input.GetKey("d")) {
            Movement.x += 1.0f;
        }        
        if (Input.GetKey("a")) {
            Movement.x -= 1.0f;
        }
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
}
