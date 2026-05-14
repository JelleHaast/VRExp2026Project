using UnityEngine;

public class Narration1Script : MonoBehaviour
{
    [Header("Instellingen")]
    [Tooltip("Sleep hier de AudioSource in die het radiobericht moet afspelen")]
    public AudioSource radioAudioSource;

    [Tooltip("Aantal seconden wachten voordat het bericht start")]
    public float wachttijd = 10f;

    void Start()
    {
        // We gebruiken Invoke om een functie na een bepaalde tijd aan te roepen
        Invoke("SpeelBerichtAf", wachttijd);

        Debug.Log("⏱️ Timer gestart: Radiobericht speelt over " + wachttijd + " seconden.");
    }

    void SpeelBerichtAf()
    {
        // Controleer eerst of er wel een AudioSource is ingesteld
        if (radioAudioSource != null)
        {
            radioAudioSource.Play();
            Debug.Log("📻 Radiobericht speelt nu af!");
        }
        else
        {
            Debug.LogError("⚠️ Je bent vergeten de AudioSource in het script te slepen!");
        }
    }
}