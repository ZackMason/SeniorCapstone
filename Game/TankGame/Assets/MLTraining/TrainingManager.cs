using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public GameObject TankPrefab;

    public List<GameObject> Tanks = new List<GameObject>();

    void Update() {
        var c1 = Tanks.Count;

        for (int i = 0; i < c1; i++) {
            if (Tanks[i] == null) {
                Tanks[i] = Instantiate(TankPrefab, transform.position, Quaternion.identity);
                Tanks[i].transform.SetParent(transform.parent);
            }
        }
    }
}
