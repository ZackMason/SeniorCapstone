using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKnockback : MonoBehaviour
{
    [SerializeField] private float knockbackStrength;

    private void OnCollision(Collision collision){
        Rigidbody tankObject = collision.collider.GetComponent<Rigidbody>();
        
        if(tankObject != null){
            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0;
            tankObject.AddForce(direction.normalized * knockbackStrength, ForceMode.Impulse);
        }
    }
}
