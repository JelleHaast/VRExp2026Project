using UnityEngine;
using System.Collections;

public class NarratorTimer : MonoBehaviour
{
    private AudioSource mijnAudio;

    [Header("Andere Triggers om naar te luisteren")]
    public AudioSource trigger1Audio;
    public AudioSource trigger2Audio;

    void Start()
    {
        mijnAudio = GetComponent<AudioSource>();
        StartCoroutine(WachtEnSpeelAf());
    }

    IEnumerator WachtEnSpeelAf()
    {
        // 1. Wacht eerst braaf de 10 seconden af
        yield return new WaitForSeconds(10f);

        // 2. Check of een van de andere twee triggers TOEVALLIG al aan het praten is.
        // Zo ja? Dan pauzeert dit script hier totdat ze allebei stil zijn!
        yield return new WaitWhile(() => 
            (trigger1Audio != null && trigger1Audio.isPlaying) || 
            (trigger2Audio != null && trigger2Audio.isPlaying)
        );

        // 3. Nu is de kust veilig, speel het geluid af!
        if (mijnAudio != null && mijnAudio.clip != null)
        {
            mijnAudio.Play();
            Debug.Log("📻 Timer-narrator start met praten na 10 seconden (en nadat anderen stil zijn)!");
        }
    }
}