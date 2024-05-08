using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public GameObject[] spawnPoints;
    public float spawnInterval = 5f; // Интервал между спаунами
    private bool canSpawn = true;
    private int currentIndex = 0;

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }

    void SpawnObject()
    {
        if (objectsToSpawn.Length == 0 || spawnPoints.Length == 0 || !canSpawn)
        {
            return;
        }

        int objectIndex = Random.Range(0, objectsToSpawn.Length);
        GameObject objectToSpawn = objectsToSpawn[objectIndex];

        Vector3 spawnPosition = spawnPoints[currentIndex].transform.position;
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        currentIndex = (currentIndex + 1) % spawnPoints.Length;
    }
}