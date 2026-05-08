using UnityEngine;
using System.Collections; // Belangrijk voor het wachten

public class KapotLicht : MonoBehaviour
{
    [Header("Sleep hier je lamp in")]
    public Light defecteLamp;

    [Header("Flikker Snelheid")]
    public float minTijd = 0.05f; // De kortste tijd dat hij aan/uit mag staan
    public float maxTijd = 0.3f;  // De langste tijd dat hij aan/uit mag staan

    void Start()
    {
        // Als we de game starten, beginnen we direct met de flikker-loop
        StartCoroutine(FlikkerEffect());
    }

    IEnumerator FlikkerEffect()
    {
        while (true) // Dit zorgt ervoor dat hij oneindig blijft doorgaan
        {
            // Zet de lamp UIT als hij AAN was, of AAN als hij UIT was
            defecteLamp.enabled = !defecteLamp.enabled;

            // Bedenk een willekeurig getal tussen de minTijd en maxTijd
            float wachttijd = Random.Range(minTijd, maxTijd);

            // Wacht exact dat willekeurige aantal seconden voordat de loop opnieuw draait
            yield return new WaitForSeconds(wachttijd);
        }
    }
}