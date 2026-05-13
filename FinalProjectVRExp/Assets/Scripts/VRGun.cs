using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

public class VRGun : MonoBehaviour
{
    [Header("Status")]
    public bool isGeladen = false;

    [Header("Schiet Instellingen")]
    public GameObject kogelPrefab;
    public Transform schietPunt;
    public float schietKracht = 20f;

    [Header("Besturing (Direct)")]
    public InputActionProperty rechterTrekkerActie; 
    public InputActionProperty linkerTrekkerActie;

    private XRGrabInteractable grabInteractable;
    private bool wordtVastgehouden = false;
    private bool heeftGeschotenDezeKlik = false;

    void Start()
    {
        Debug.Log("🔫 [DEBUG] Start() - VRGeweer (Beide Handen) is ontwaakt.");
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        // Zet rechter microfoon aan
        if (rechterTrekkerActie.action != null) 
        {
            rechterTrekkerActie.action.Enable();
            Debug.Log("🔫 [DEBUG] Rechter trekker succesvol geactiveerd!");
        }
        else Debug.LogWarning("🔴 [WAARSCHUWING] Rechter Trekker is niet ingevuld in de Inspector!");

        // Zet linker microfoon aan
        if (linkerTrekkerActie.action != null) 
        {
            linkerTrekkerActie.action.Enable();
            Debug.Log("🔫 [DEBUG] Linker trekker succesvol geactiveerd!");
        }
        else Debug.LogWarning("🔴 [WAARSCHUWING] Linker Trekker is niet ingevuld in de Inspector!");

        if (grabInteractable != null) 
        {
            grabInteractable.selectEntered.AddListener(Oppakken);
            grabInteractable.selectExited.AddListener(Loslaten);
        } 
    }

    void Update()
    {
        if (wordtVastgehouden)
        {
            // Lees beide trekkers uit (als ze zijn ingevuld, anders is het 0)
            float drukRechts = (rechterTrekkerActie.action != null) ? rechterTrekkerActie.action.ReadValue<float>() : 0f;
            float drukLinks = (linkerTrekkerActie.action != null) ? linkerTrekkerActie.action.ReadValue<float>() : 0f;
            
            // We pakken de hoogste waarde
            float trekkerWaarde = Mathf.Max(drukRechts, drukLinks);
            
            if (trekkerWaarde > 0.5f && !heeftGeschotenDezeKlik)
            {
                Debug.Log("🔫 [DEBUG] PANG! Trekker overgehaald!");
                heeftGeschotenDezeKlik = true;
                HaalTrekkerOver();
            }
            else if (trekkerWaarde < 0.1f)
            {
                if (heeftGeschotenDezeKlik) Debug.Log("🔫 [DEBUG] Trekker losgelaten, klaar voor volgend schot.");
                heeftGeschotenDezeKlik = false; // Reset als we loslaten
            }
        }
    }

    void Oppakken(SelectEnterEventArgs args)
    {
        wordtVastgehouden = true;
        Debug.Log("🔫 [DEBUG] Geweer VASTGEPAKT door: " + args.interactorObject.transform.name);
    }

    void Loslaten(SelectExitEventArgs args)
    {
        wordtVastgehouden = false;
        Debug.Log("🔫 [DEBUG] Geweer LOSGELATEN.");
    }

    public void HaalTrekkerOver()
    {
        if (isGeladen)
        {
            Schiet();
        }
        else
        {
            Debug.Log("🔫 [DEBUG] KLIK! Het geweer is nog leeg...");
        }
    }

    void Schiet()
    {
        if (kogelPrefab == null) 
        {
            Debug.LogError("🔴 [FOUT] De KOGEL mist op het object genaamd: " + gameObject.name);
            return;
        }
        if (schietPunt == null) 
        {
            Debug.LogError("🔴 [FOUT] Het SCHIETPUNT mist op het object genaamd: " + gameObject.name);
            return;
        }

        GameObject nieuweKogel = Instantiate(kogelPrefab, schietPunt.position, schietPunt.rotation);
        
        Rigidbody rb = nieuweKogel.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            rb.linearVelocity = schietPunt.forward * schietKracht;
            Debug.Log("🔫 [DEBUG] Kogel succesvol afgevuurd!");
        }
    }

   void OnTriggerEnter(Collider anderObject)
    {
        if (anderObject.CompareTag("Munitie") && !isGeladen)
        {
            // Zoek het geheugen-script op de kogel die we net aanraken
            BulletTransformation status = anderObject.GetComponent<BulletTransformation>();

            // Heeft de kogel het script en zit de eindstof erop?
            if (status != null && status.heeftEindstof == true)
            {
                isGeladen = true;
                Debug.Log("🔫 [DEBUG] Kogel MÉT eindstof erin gestopt! Geweer is nu GELADEN.");
                Destroy(anderObject.gameObject); 
            }
            else if (status != null && status.heeftEindstof == false)
            {
                Debug.Log("⚠️ [DEBUG] Fout! Deze kogel heeft nog geen eindstof gekregen! Het geweer weigert hem.");
                // De kogel wordt nu niet vernietigd, hij stuitert gewoon weg of blijft liggen.
            }
        }
    }
}