using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public QuestData questLamp;
    public QuestData questRadar;
    public QuestData questPoison;
    public Transform respawnPoint1;
    public Transform respawnPoint2;
    public Transform respawnPoint3;

    public Transform player;

    private Transform _currentRespawnPoint;

    void Start()
    {
        _currentRespawnPoint = respawnPoint1; // default
    }

    // Call this from your quest completion logic
    public void UpdateRespawnPoint()
    {
        if (questPoison.isCompleted)
            _currentRespawnPoint = respawnPoint3;
        else if (questRadar.isCompleted)
            _currentRespawnPoint = respawnPoint2;
        else if (questLamp.isCompleted)
            _currentRespawnPoint = respawnPoint1;
    }

    public void Respawn()
    {
        player.position = _currentRespawnPoint.position;
    }
}