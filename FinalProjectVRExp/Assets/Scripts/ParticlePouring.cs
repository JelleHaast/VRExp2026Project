using UnityEngine;

public class ParticlePouring : MonoBehaviour
{
    [Header("Instellingen")]
    public float gietHoek = 95f; // Vanaf 90 graden gaat hij gieten
    
    private ParticleSystem straal;

    void Start()
    {
        // Zoek bij de start direct het Particle System dat in deze fles zit
        straal = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        // Als er geen particles zijn, doe dan niks
        if (straal == null) return;

        // Bereken elke frame hoe schuin DEZE fles hangt
        float hoek = Vector3.Angle(transform.up, Vector3.up);

        if (hoek >= gietHoek)
        {
            // Fles is schuin! Zet de straal aan.
            if (!straal.isPlaying) straal.Play();
        }
        else
        {
            // Fles is weer recht! Zet de straal uit.
            if (straal.isPlaying) straal.Stop();
        }
    }
}