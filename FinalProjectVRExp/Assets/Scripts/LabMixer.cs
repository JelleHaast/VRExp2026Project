using UnityEngine;
using TMPro; // Dit is nodig om TextMeshPro aan te sturen!

public class ScheikundeMixer : MonoBehaviour
{
    [Header("Voortgang")]
    public bool heeftStofA = false;
    public bool heeftStofB = false;

    [Header("Instellingen")]
    public float gietHoek = 100f; 
    public GameObject objectDatVerdwijnt;
    public GameObject objectDatVerschijnt;

    [Header("In-Game Debugger")]
    public TextMeshPro debugScherm; // Sleep hier je zwevende tekst in
    private string laatsteMelding = "Wachten op glazen...";
    private float actueleKanteling = 0f;

    void Update()
    {
        // Als we een debug-scherm hebben gekoppeld, werk deze dan elke frame bij!
        if (debugScherm != null)
        {
            debugScherm.text = 
                "<color=yellow>--- MIXER DEBUGGER ---</color>\n" +
                "Stof A: " + (heeftStofA ? "<color=green>JA</color>" : "<color=red>NEE</color>") + "\n" +
                "Stof B: " + (heeftStofB ? "<color=green>JA</color>" : "<color=red>NEE</color>") + "\n\n" +
                "Kanteling glas: " + Mathf.RoundToInt(actueleKanteling) + " graden\n" +
                "Log: " + laatsteMelding;
        }
    }

    void OnTriggerStay(Collider anderObject)
    {
        // Update de live kanteling-meter voor de debugger
        actueleKanteling = Vector3.Angle(anderObject.transform.up, Vector3.up);

        if (actueleKanteling >= gietHoek)
        {
            CheckFlesje(anderObject);
        }
    }

    void OnTriggerExit(Collider anderObject)
    {
        // Zet de hoek-meter weer op 0 als we het glas weghalen
        actueleKanteling = 0f;
    }

    void CheckFlesje(Collider flesje)
    {
        if (flesje.CompareTag("StofA") && !heeftStofA)
        {
            heeftStofA = true;
            GietSucces(flesje.gameObject, "Stof A");
        }
        else if (flesje.CompareTag("StofB") && !heeftStofB)
        {
            heeftStofB = true;
            GietSucces(flesje.gameObject, "Stof B");
        }
    }

    void GietSucces(GameObject flesje, string stofNaam)
    {
        laatsteMelding = stofNaam + " is met succes gegoten!";
        flesje.tag = "Untagged"; 
        CheckOfMinigameKlaarIs();
    }

    void CheckOfMinigameKlaarIs()
    {
        if (heeftStofA && heeftStofB)
        {
            laatsteMelding = "SUCCES! De eindstof is gemaakt!";
            
            if (objectDatVerdwijnt != null) objectDatVerdwijnt.SetActive(false);
            if (objectDatVerschijnt != null) objectDatVerschijnt.SetActive(true);
        }
    }
}