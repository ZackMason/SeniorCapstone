using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0, 200)]
    public float MaxHealth;

    private float _currentHealth;
    private Destructible _destructible;


    void Start() {
        _currentHealth = MaxHealth;
        _destructible = GetComponentInChildren<Destructible>();
    }

    public void Damage(float damage) {
        _currentHealth -= damage;

        if (_currentHealth <= 0.0f) {
            if (_destructible != null) {
                _destructible.Destruct();
                Timer.score += 100;
            }
            Destroy(this.gameObject);
        }
    }
}
