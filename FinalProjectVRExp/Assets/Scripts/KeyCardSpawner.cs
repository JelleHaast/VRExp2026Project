using System.Collections.Generic;
using UnityEngine;

public class KeyCardSpawner : MonoBehaviour
{
    public Transform spawnPointsParent;
    public GameObject prefab;
    public bool matchRotation = true;
    public bool parentToSpawnPoint = false;
    public bool spawnOnStart = true;

    private GameObject _spawnedObject;

    private void Start()
    {
        if (spawnOnStart)
            Spawn();
    }

    public void Spawn()
    {
        Clear();

        if (spawnPointsParent == null || prefab == null) return;

        List<Transform> spawnPoints = new();
        foreach (Transform child in spawnPointsParent)
            spawnPoints.Add(child);

        if (spawnPoints.Count == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Quaternion rotation = matchRotation ? spawnPoint.rotation : Quaternion.identity;
        Transform parent = parentToSpawnPoint ? spawnPoint : null;

        _spawnedObject = Instantiate(prefab, spawnPoint.position, rotation, parent);
    }

    public void Clear()
    {
        if (_spawnedObject != null)
            Destroy(_spawnedObject);
    }
}