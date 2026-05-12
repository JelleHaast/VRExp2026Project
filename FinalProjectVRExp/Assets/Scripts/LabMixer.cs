using UnityEngine;
using TMPro;

public class LabMixer : MonoBehaviour
{
    [Header("Voortgang")]
    public bool heeftStofA = false;
    public bool heeftStofB = false;
    public bool heeftStofC = false;

    [Header("Instellingen")]
    public float gietHoek = 100f; 
    public GameObject objectDatVerdwijnt;
    public GameObject objectDatVerschijnt;

    private float volgendeLogTijd = 0f;

    void OnTriggerStay(Collider anderObject)
    {
        ParticleSystem straal = anderObject.GetComponentInChildren<ParticleSystem>();
        
        // Als dit object geen Particle System heeft, negeer het dan!
        if (straal == null) return; 

        float hoek = Vector3.Angle(anderObject.transform.up, Vector3.up);

        // 📡 RADAR: Print elke halve seconde wat de code ziet
        if (Time.time > volgendeLogTijd)
        {
            Debug.Log("📡 RADAR: Ik zie [" + anderObject.name + "]! De hoek is: " + Mathf.Round(hoek) + " graden.");
            volgendeLogTijd = Time.time + 0.5f;
        }

        if (hoek >= gietHoek)
        {
            if (!straal.isPlaying) straal.Play();
            CheckFlesje(anderObject);
        }
        else
        {
            if (straal.isPlaying) straal.Stop();
        }
    }

    void OnTriggerExit(Collider anderObject)
    {
        ParticleSystem straal = anderObject.GetComponentInChildren<ParticleSystem>();
        if (straal != null && straal.isPlaying)
        {
            straal.Stop();
        }
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
        else if (flesje.CompareTag("StofC") && !heeftStofC)
        {
            heeftStofC = true;
            GietSucces(flesje.gameObject, "Stof C");
        }
    }

    void GietSucces(GameObject flesje, string stofNaam)
    {
        Debug.Log("🧪 " + stofNaam + " is met succes gegoten!");
        flesje.tag = "Untagged"; 
        CheckOfMinigameKlaarIs();
    }

    void CheckOfMinigameKlaarIs()
    {
        if (heeftStofA && heeftStofB && heeftStofC)
        {
            Debug.Log("🎉 SUCCES! Alle stoffen zijn gemixt!");
            if (objectDatVerdwijnt != null) objectDatVerdwijnt.SetActive(false);
            if (objectDatVerschijnt != null) objectDatVerschijnt.SetActive(true);
        }
    }
}