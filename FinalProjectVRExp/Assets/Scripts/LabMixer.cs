using UnityEngine;
using TMPro;

public class LabMixer : MonoBehaviour
{
    [Header("Voortgang")]
    public bool heeftStofA = false;
    public bool heeftStofB = false;
    public bool heeftStofC = false;
    private int aantalGegoten = 0; 

    [Header("Instellingen Gieten")]
    public float gietHoek = 100f; 
    [Tooltip("Hoeveel seconden moet de speler de fles gieten?")]
    public float benodigdeGietTijd = 1.0f; // Hier staat hij op 1 seconde
    
    // Deze variabele houdt stiekem op de achtergrond de tijd bij
    private float huidigeGietTimer = 0f; 

    [Header("Instellingen Eindresultaat")]
    public GameObject objectDatVerdwijnt;
    public GameObject objectDatVerschijnt;

    [Header("Vloeistof Niveaus (Vulniveau)")]
    public GameObject niveau1;
    public GameObject niveau2;


    private float volgendeLogTijd = 0f;

    void OnTriggerStay(Collider anderObject)
    {
        ParticleSystem straal = anderObject.GetComponentInChildren<ParticleSystem>();
        if (straal == null) return; 

        float hoek = Vector3.Angle(anderObject.transform.up, Vector3.up);

        if (Time.time > volgendeLogTijd)
        {
            Debug.Log("📡 RADAR: Ik zie [" + anderObject.name + "]. Hoek: " + Mathf.Round(hoek) + " graden.");
            volgendeLogTijd = Time.time + 0.5f;
        }

        if (hoek >= gietHoek)
        {
            // De fles is schuin genoeg, start de particle spray!
            if (!straal.isPlaying) straal.Play();
            
            // Check of dit een fles is die we nog kunnen gebruiken (bijv. niet al leeg is)
            if (IsGeldigeFles(anderObject))
            {
                // Tel de milliseconden op zolang we gieten
                huidigeGietTimer += Time.deltaTime;
                
                // Zijn we over de 1 seconde heen?
                if (huidigeGietTimer >= benodigdeGietTijd)
                {
                    CheckFlesje(anderObject);
                    huidigeGietTimer = 0f; // Reset de timer voor de volgende fles!
                }
            }
        }
        else
        {
            // De speler houdt de fles weer recht
            if (straal.isPlaying) straal.Stop();
            
            // Straf de speler: als je te vroeg stopt met gieten, begint de timer weer op 0
            huidigeGietTimer = 0f; 
        }
    }

    void OnTriggerExit(Collider anderObject)
    {
        ParticleSystem straal = anderObject.GetComponentInChildren<ParticleSystem>();
        if (straal != null && straal.isPlaying)
        {
            straal.Stop();
        }
        
        // Fles is helemaal weg, reset de timer voor de zekerheid
        huidigeGietTimer = 0f; 
    }

    // Handige check om te zien of we de timer wel moeten laten lopen
    bool IsGeldigeFles(Collider flesje)
    {
        if (flesje.CompareTag("StofA") && !heeftStofA) return true;
        if (flesje.CompareTag("StofB") && !heeftStofB) return true;
        if (flesje.CompareTag("StofC") && !heeftStofC) return true;
        return false; // Geen geldige stof of al gegoten
    }

    void CheckFlesje(Collider flesje)
    {
        bool nieuwGegoten = false;
        string stofNaam = "";

        if (flesje.CompareTag("StofA") && !heeftStofA)
        {
            heeftStofA = true;
            nieuwGegoten = true;
            stofNaam = "Stof A";
        }
        else if (flesje.CompareTag("StofB") && !heeftStofB)
        {
            heeftStofB = true;
            nieuwGegoten = true;
            stofNaam = "Stof B";
        }
        else if (flesje.CompareTag("StofC") && !heeftStofC)
        {
            heeftStofC = true;
            nieuwGegoten = true;
            stofNaam = "Stof C";
        }

        if (nieuwGegoten)
        {
            aantalGegoten++; 
            VerhoogVloeistof(); 
            GietSucces(flesje.gameObject, stofNaam);
        }
    }

    void VerhoogVloeistof()
    {
        if (aantalGegoten == 1 && niveau1 != null) niveau1.SetActive(true);
        if (aantalGegoten == 2 && niveau2 != null) niveau2.SetActive(true);
    }

    void GietSucces(GameObject flesje, string stofNaam)
    {
        Debug.Log("🧪 " + stofNaam + " is met succes gegoten!");
        flesje.tag = "Untagged"; // Maakt de fles leeg voor de rest van de game
        CheckOfMinigameKlaarIs();
    }

    void CheckOfMinigameKlaarIs()
    {
        if (heeftStofA && heeftStofB && heeftStofC)
        {
            Debug.Log("🎉 SUCCES! Alle stoffen zijn gemixt!");
            
            if (niveau1 != null) niveau1.SetActive(false);
            if (niveau2 != null) niveau2.SetActive(false);

            if (objectDatVerdwijnt != null) objectDatVerdwijnt.SetActive(false);
            if (objectDatVerschijnt != null) objectDatVerschijnt.SetActive(true);
        }
    }
}