using UnityEngine;
using System.Collections; // Nodig voor de timer (Coroutines)

public class MonsterSpawner : MonoBehaviour
{
    [Header("Instellingen")]
    [Tooltip("Sleep hier de blauwe Prefab van je monster in")]
    public GameObject monsterPrefab;
    
    [Tooltip("Waar moet het monster spawnen?")]
    public Transform spawnPunt;
    
    [Tooltip("Tijd in seconden VOORDAT hij respawnt nadat het lijk verdwijnt")]
    public float respawnWachttijd = 10f;

    private GameObject huidigMonster;
    private bool isAanHetSpawnen = false;

    void Start()
    {
        // Spawn direct het eerste monster als de game begint
        SpawnNieuwMonster();
    }

    void Update()
    {
        // Controleer of het monster dood/vernietigd is, en of we niet al aan het aftellen zijn
        if (huidigMonster == null && !isAanHetSpawnen)
        {
            StartCoroutine(StartRespawnTimer());
        }
    }

    IEnumerator StartRespawnTimer()
    {
        isAanHetSpawnen = true;
        Debug.Log("Monster is weg! Start timer van " + respawnWachttijd + " seconden...");

        // Wacht het ingestelde aantal seconden
        yield return new WaitForSeconds(respawnWachttijd);

        // Timer is klaar, spawn het monster!
        SpawnNieuwMonster();
        
        isAanHetSpawnen = false; // Reset de check
    }

    void SpawnNieuwMonster()
    {
        if (monsterPrefab != null && spawnPunt != null)
        {
            // Maak een nieuwe kopie van de prefab op de locatie van het spawnpunt
            huidigMonster = Instantiate(monsterPrefab, spawnPunt.position, spawnPunt.rotation);
            Debug.Log("👾 Nieuwe Axyl is gespawnd!");
        }
        else
        {
            Debug.LogWarning("⚠️ Spawner mist een Prefab of een Spawnpunt!");
        }
    }
}