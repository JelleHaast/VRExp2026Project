using UnityEngine;

public class MonsterSteps : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        // Zet het geluid direct aan zodra dit script/monster in de wereld verschijnt
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}