using UnityEngine;

public class LightAlarm : MonoBehaviour
{
    public Light noodLicht;
    public float knipperSnelheid = 5f;
    public float maxFelheid = 10f;

    void Update()
    {
        // Mathf.PingPong zorgt ervoor dat de waarde soepel heen en weer stuitert tussen 0 en de maxFelheid
        noodLicht.intensity = Mathf.PingPong(Time.time * knipperSnelheid, maxFelheid);
    }
}