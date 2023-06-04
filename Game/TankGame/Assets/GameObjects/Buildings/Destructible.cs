using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Destructible : MonoBehaviour
{
    public GameObject DestroyedPrefab;

    [SerializeField] private CinemachineImpulseSource _impulsor;

    void Start() {
        _impulsor = GetComponentInChildren<CinemachineImpulseSource>();
    }

    [Range(0, 10000)]
    public float RamResistance;

    public void Destruct() {
        if (DestroyedPrefab) {
            var savedScale = transform.localScale;
            var newObject = Instantiate(DestroyedPrefab, transform.position, transform.rotation);
            newObject.transform.localScale = savedScale;

            if (_impulsor) { // todo(zack): if we're spawning visuals, lets add camera shake
                
                var target = RespawnManager.Instance.Player.transform.position;
                _impulsor.GenerateImpulse( (transform.position - target).normalized * 0.5f);
            }
        }
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision col)
    {
        Rigidbody otherBody = col.rigidbody;
        Rigidbody body = GetComponent<Rigidbody>();
        float otherKineticEnergy = 0f;
        float selfKineticEnergy = 0f;

        if (body == null) {
            body = GetComponentInParent<Rigidbody>();
        }
        
        if (body != null) {
            selfKineticEnergy = body.velocity.magnitude * body.velocity.magnitude * body.mass * 0.5f;
        }
        if (otherBody != null) {
            otherKineticEnergy = otherBody.velocity.magnitude * otherBody.velocity.magnitude * otherBody.mass * 0.5f;
        }

        float totalKineticEnergy = (otherKineticEnergy + selfKineticEnergy);
        if (totalKineticEnergy >= RamResistance) {
            Destruct();

            ExplosionManager.Instance.SpawnExplosion(col.contacts[0].point, 2.0f, totalKineticEnergy * 0.05f, 1.0f);
        }
    }
}
