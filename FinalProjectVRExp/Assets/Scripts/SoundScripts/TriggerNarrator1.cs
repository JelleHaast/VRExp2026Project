using UnityEngine;

public class TriggerNarrator1 : MonoBehaviour
{
    [Header("Audio Instellingen")]
    public AudioSource radioAudioSource; // De AudioSource van je speler/radio
    public AudioClip storageRoomBericht; // Het nieuwe audiofragment

    private bool isAlAfgespeeld = false; // Zorgt dat hij het maar 1 keer zegt

    // Deze functie vuurt af zodra IETS de onzichtbare box raakt
    void OnTriggerEnter(Collider other)
    {
        // 1. Check of de stem al geweest is
        if (isAlAfgespeeld) return;

        // 2. Check of het wel ECHT de speler is die erdoorheen loopt 
        // (en niet een kogel of een monster)
        if (other.CompareTag("Hider") || other.CompareTag("MainCamera"))
        {
            if (radioAudioSource != null && storageRoomBericht != null)
            {
                // Vervang het huidige geluid en speel af
                radioAudioSource.clip = storageRoomBericht;
                radioAudioSource.Play();
                
                isAlAfgespeeld = true; // Zet hem op true zodat hij niet blijft herhalen
            }
        }
    }
}