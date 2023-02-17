using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBasket : MonoBehaviour
{
    private BoxCollider _boxCollider;


    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item") {
            ScoreManager.Instance.PlayerScore += 1;
            Destroy(other.gameObject);
        }
    }

}
