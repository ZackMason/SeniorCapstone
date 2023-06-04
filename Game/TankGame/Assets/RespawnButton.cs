using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnButton : MonoBehaviour
{
    public void RespawnPlayer() => RespawnManager.Instance?.KillPlayer(); 
}
