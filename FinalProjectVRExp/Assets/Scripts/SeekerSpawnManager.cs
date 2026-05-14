using UnityEngine;
using Unity.MLAgents;

public class SeekerSpawnManager : MonoBehaviour
{
    [SerializeField] private Agent monster;
    [SerializeField] private Transform spawnPoint;

    public void Spawn()
    {
        monster.gameObject.SetActive(true);
        monster.transform.position = spawnPoint.position;
        monster.OnEpisodeBegin();
    }

    public void Despawn()
    {
        monster.gameObject.SetActive(false);
    }
}