using System.Collections;
using UnityEngine;

public class MonsterPassive : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] monsterSounds; // Dit maakt een lijstje (array) in Unity

    void Start()
    {
        // Als je vergeten bent de AudioSource te koppelen, zoekt hij hem zelf op
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Start de oneindige loop
        StartCoroutine(PlaySoundRoutine());
    }

    IEnumerator PlaySoundRoutine()
    {
        // De while(true) zorgt ervoor dat hij dit blijft doen zolang de game draait
        while (true)
        {
            // 1. Kies een willekeurige wachttijd tussen de 20 en 40 seconden
            float waitTime = Random.Range(20f, 40f);
            
            // 2. Wacht zolang op de achtergrond
            yield return new WaitForSeconds(waitTime);

            // 3. Controleer of je de lijst wel hebt gevuld met geluiden
            if (monsterSounds.Length > 0)
            {
                // Kies een willekeurig getal uit je lijst
                int randomIndex = Random.Range(0, monsterSounds.Length);
                AudioClip randomClip = monsterSounds[randomIndex];

                // Speel het willekeurige geluid één keer af
                audioSource.PlayOneShot(randomClip);
            }
        }
    }
}