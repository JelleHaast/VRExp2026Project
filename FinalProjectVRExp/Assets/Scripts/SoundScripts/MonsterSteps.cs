using UnityEngine;

public class MonsterSteps : MonoBehaviour
{
    public AudioSource audioSource;
    
    // We onthouden hier de positie van de vorige frame
    private Vector3 vorigePositie;

    void Start()
    {
        // Sla de startpositie op zodra de game begint
        // (We gebruiken transform.root.position zodat we altijd naar het hele monster kijken, 
        // zelfs als dit script op een onderdeeltje zoals een voet staat)
        vorigePositie = transform.root.position;
    }

    void Update()
    {
        // 1. Bereken de afstand tussen de huidige plek en de plek van de vorige frame
        float afstandBeweegt = Vector3.Distance(transform.root.position, vorigePositie);
        
        // 2. Reken dit om naar een echte snelheid (onafhankelijk van framerate)
        float snelheid = afstandBeweegt / Time.deltaTime;

        // 3. Check of de snelheid hoog genoeg is om als 'lopen' te tellen
        if (snelheid > 0.1f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause(); 
            }
        }

        // 4. Update de vorige positie voor de check in de volgende frame!
        vorigePositie = transform.root.position;
    }
}