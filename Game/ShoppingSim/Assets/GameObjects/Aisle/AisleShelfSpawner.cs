using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AisleShelfSpawner : MonoBehaviour
{
    private GameObject _spawner = null;

    public GameObject[] CanSpawn;

    [Range(0.0f, 20.0f)]
    public float SpawnDistance;

    [Range(0, 20)]
    public int SpawnCount;

    void SpawnShelf(GameObject shelf) {

        Vector3 SpawnPosition = shelf.transform.position;

        Matrix4x4 m = shelf.transform.worldToLocalMatrix;
        Vector3 SpawnDirection = new Vector3(m[2, 0], m[2, 1], m[2, 2]);

        // todo(zack): replace this
        for (int i = 0; i < SpawnCount; i++) {
            int SpawnIndex = Random.Range(0, CanSpawn.Length);
            Instantiate(CanSpawn[SpawnIndex], SpawnPosition, Quaternion.identity);
            
            SpawnPosition += SpawnDirection * SpawnDistance;
        }
    }

    void Start() {
        _spawner = this.gameObject.transform.Find("SpawnStarts").gameObject;

        for (int i = 0; i < _spawner.transform.childCount; i++)
        {
            SpawnShelf(_spawner.transform.GetChild(i).gameObject);
        }
    }

}
