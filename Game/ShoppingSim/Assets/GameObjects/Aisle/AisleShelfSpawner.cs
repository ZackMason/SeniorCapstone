using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AisleShelfSpawner : MonoBehaviour
{
    public GameObject[] CanSpawn;
    private GameObject _spawner = null;

    [Range(0.0f, 2.0f)]
    public float SpawnDistance;
    
    private float _shelfDistance;

    void SpawnShelf(GameObject shelf) {
        int SpawnIndex = Random.Range(0, CanSpawn.Length);

        Vector3 SpawnPosition = shelf.transform.position;

        Vector3 SpawnDirection = new Vector3(0,0,1);

        // todo(zack): replace this
        for (int i = 0; i < 10; i++) {
            Instantiate(CanSpawn[SpawnIndex], SpawnPosition, Quaternion.identity);
            
            SpawnPosition += SpawnDirection * SpawnDistance;
        }
    }

    void Start()
    {
        _shelfDistance = GetComponent<BoxCollider>().bounds.size.z;
        _spawner = this.gameObject.transform.GetChild(0).gameObject;

        SpawnShelf(_spawner.transform.GetChild(0).gameObject);
        SpawnShelf(_spawner.transform.GetChild(1).gameObject);
        SpawnShelf(_spawner.transform.GetChild(2).gameObject);

        SpawnShelf(_spawner.transform.GetChild(3).gameObject);
        SpawnShelf(_spawner.transform.GetChild(4).gameObject);
        SpawnShelf(_spawner.transform.GetChild(5).gameObject);
    }

    
    void Update()
    {
        
    }
}
