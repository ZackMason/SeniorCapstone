using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    [Range(0, 200)]
    public float MaxHealth;
    
    private float _currentHealth;
    private Destructible _destructible;

    void Start() {
        _currentHealth = MaxHealth;
        _destructible = GetComponentInChildren<Destructible>();
    }

    void Update() {
        if (healthText != null) {
            healthText.text = MaxHealth.ToString();
        }
    }    

    public bool Damage(float damage) {
        _currentHealth -= damage;

        if (_currentHealth <= 0.0f) {
            if (_destructible != null) {
                _destructible.Destruct();
                Timer.score += 100;
            }
            Destroy(this.gameObject);
            return true;
        }
        return false;
    }
}
