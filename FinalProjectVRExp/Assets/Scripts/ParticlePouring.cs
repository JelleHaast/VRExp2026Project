using UnityEngine;

public class ParticlePouring : MonoBehaviour
{
    [Header("Instellingen")]
    public float gietHoek = 90f;
    public ParticleSystem vloeistofParticles; // Sleep hier je Particle System in!

    void Update()
    {
        // We checken of de beker ver genoeg gekanteld is
        // (Afhankelijk van hoe je model is gedraaid, moet je 'up' of 'right' gebruiken)
        float huidigeHoek = Vector3.Angle(Vector3.up, transform.up);

        if (huidigeHoek > gietHoek)
        {
            if (!vloeistofParticles.isPlaying)
            {
                vloeistofParticles.Play();
                Debug.Log("💧 [DEBUG] Aan het gieten!");
            }
        }
        else
        {
            if (vloeistofParticles.isPlaying)
            {
                vloeistofParticles.Stop();
            }
        }
    }
}