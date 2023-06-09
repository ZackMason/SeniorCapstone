using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note(Zack): This Script activates the boss when the player enters the trigger area
public class BossSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _boss = new List<GameObject>();
    [SerializeField] private GameObject _healthContainer;

    void Start() {
        _boss.ForEach(boss => boss.SetActive(false));
    } 
        
    void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody?.tag == "Player") {
            _healthContainer.SetActive(true);
            _boss.ForEach(boss => boss.SetActive(true));
            Destroy(this);
        }
    }
}
