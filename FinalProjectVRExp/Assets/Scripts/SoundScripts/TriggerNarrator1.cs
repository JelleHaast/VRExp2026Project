using UnityEngine;
using System.Collections;

public class TriggerNarrator : MonoBehaviour
{
    private AudioSource mijnAudio;
    private bool isGetriggerd = false;

    [Header("De ANDERE twee Narrator AudioSources")]
    public AudioSource andereNarratorA;
    public AudioSource andereNarratorB;

    void Start()
    {
        mijnAudio = GetComponent<AudioSource>();
        if (mijnAudio == null) Debug.LogError("❌ [FOUT] " + gameObject.name + " mist een AudioSource component!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGetriggerd) return;

        if (other.CompareTag("Hider") || other.CompareTag("MainCamera"))
        {
            isGetriggerd = true;
            Debug.Log("🚶‍♂️ [STAP 1] De speler liep in de trigger van: " + gameObject.name);
            StartCoroutine(WachtOpBeurtEnSpeelAf());
        }
    }

    IEnumerator WachtOpBeurtEnSpeelAf()
    {
        Debug.Log("⏳ [STAP 2] " + gameObject.name + " checkt of de andere narrators aan het praten zijn...");

        // Blijf wachten ZOLANG narrator A óf narrator B aan het praten is
        yield return new WaitWhile(() => 
            (andereNarratorA != null && andereNarratorA.isPlaying) || 
            (andereNarratorB != null && andereNarratorB.isPlaying)
        );

        Debug.Log("✅ [STAP 3] De rest is stil! " + gameObject.name + " mag nu beginnen.");

        if (mijnAudio != null && mijnAudio.clip != null)
        {
            mijnAudio.Play();
            Debug.Log("🔊 [STAP 4] Geluid van " + gameObject.name + " speelt NU af!");
        }
        else
        {
            Debug.LogError("❌ [FOUT] " + gameObject.name + " heeft GEEN audiobestand (AudioClip) in zijn AudioSource staan!");
        }
    }
}