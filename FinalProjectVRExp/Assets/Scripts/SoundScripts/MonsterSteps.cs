using UnityEngine;
using UnityEngine.AI;

public class MonsterSteps : MonoBehaviour
{
    public AudioSource audioSource;
    private NavMeshAgent nav;

    void Start()
    {
        // Zoek de 'GPS' (NavMeshAgent) van het monster op de bovenliggende map
        nav = GetComponentInParent<NavMeshAgent>();
    }

    void Update()
    {
        // Checkt of het monster daadwerkelijk beweegt (snelheid is groter dan 0.1)
        if (nav.velocity.magnitude > 0.1f)
        {
            // Als hij beweegt, maar het geluid staat nog uit, zet het dan aan!
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Als hij stilstaat, pauzeer het geluid direct
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }
}