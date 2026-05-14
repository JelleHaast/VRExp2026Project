using UnityEngine;
using System.Collections;

public class JumpscareScript : MonoBehaviour
{
    [Header("Jumpscare Instellingen")]
    public GameObject horrorBeeld;
    public AudioSource screamAudio;
    public float duur = 2.0f;

    // Deze public functie kan door het andere script worden aangeroepen!
    public void StartDeJumpscare()
    {
        StartCoroutine(SpeelJumpscare());
    }

    IEnumerator SpeelJumpscare()
    {
        Debug.Log("👻 BOE! Plaatje en geluid starten!");
        if (horrorBeeld != null) horrorBeeld.SetActive(true);
        if (screamAudio != null) screamAudio.Play();

        yield return new WaitForSeconds(duur);

        if (horrorBeeld != null) horrorBeeld.SetActive(false);
    }
}