using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note(Zack): This Script activates the boss when the player enters the trigger area
public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _boss;

    void Start() => _boss.SetActive(false);
        
    void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody?.tag == "Player") {
            _boss.SetActive(true);
            Destroy(this);
        }
    }
}
