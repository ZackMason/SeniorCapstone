using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Range(0, 200)]
    public float MaxHealth;

    private float _currentHealth;

    void Start() {
        _currentHealth = MaxHealth;
    }

    public void Damage(float damage) {
        _currentHealth -= damage;
    }
}
