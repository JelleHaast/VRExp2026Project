using UnityEngine;

public class MonsterSteps : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip stepSound;

    // Deze functie wordt straks aangeroepen door je animatie
    public void PlayStep()
    {
        if (audioSource != null && stepSound != null)
        {
            // PlayOneShot zorgt ervoor dat meerdere stappen soepel door elkaar kunnen klinken
            audioSource.PlayOneShot(stepSound);
        }
    }
}