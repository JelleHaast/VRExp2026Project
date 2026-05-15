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

    [Header("Effecten")]
    public ParticleSystem schietParticles; 
    [Tooltip("Het geluid dat speelt als je de trekker overhaalt")]
    public AudioClip schietGeluid; // Ik heb deze toegevoegd voor de duidelijkheid
    [Tooltip("Het geluid dat speelt als de kogel in het wapen klikt")]
    public AudioClip laadGeluid; // DIT IS NIEUW!
    
    private AudioSource audioSource;

    [Header("Besturing")]
    public InputActionProperty rechterTrekkerActie; 
    public InputActionProperty linkerTrekkerActie;

    private XRGrabInteractable grabInteractable;
    private bool wordtVastgehouden = false;
    private bool heeftGeschotenDezeKlik = false;

    void Start()
    {
        Debug.Log("🔫 [DEBUG] Start() - VRGun is volledig opgeladen met geluid en particles.");
        
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        
        // Activeer de input acties
        if (rechterTrekkerActie.action != null) rechterTrekkerActie.action.Enable();
        if (linkerTrekkerActie.action != null) linkerTrekkerActie.action.Enable();

        if (grabInteractable != null) 
        {
            grabInteractable.selectEntered.AddListener(Oppakken);
            grabInteractable.selectExited.AddListener(Loslaten);
        } 
        
        // Zorg dat het schietgeluid standaard in de AudioSource zit, mocht je dat zo hebben ingesteld
        if (audioSource != null && audioSource.clip != null && schietGeluid == null)
        {
            schietGeluid = audioSource.clip;
        }
    }

    void Update()
    {
        if (wordtVastgehouden)
        {
            // Check de waarde van beide controllers
            float drukRechts = (rechterTrekkerActie.action != null) ? rechterTrekkerActie.action.ReadValue<float>() : 0f;
            float drukLinks = (linkerTrekkerActie.action != null) ? linkerTrekkerActie.action.ReadValue<float>() : 0f;
            float trekkerWaarde = Mathf.Max(drukRechts, drukLinks);
            
            // Schiet bij 50% indrukken
            if (trekkerWaarde > 0.5f && !heeftGeschotenDezeKlik)
            {
                heeftGeschotenDezeKlik = true;
                HaalTrekkerOver();
            }
            // Reset de trekker pas als je hem bijna helemaal loslaat
            else if (trekkerWaarde < 0.1f)
            {
                heeftGeschotenDezeKlik = false;
            }
        }
    }

    void Oppakken(SelectEnterEventArgs args) => wordtVastgehouden = true;
    void Loslaten(SelectExitEventArgs args) => wordtVastgehouden = false;

    public void HaalTrekkerOver()
    {
        if (isGeladen) 
        {
            Schiet();
        }
        else 
        {
            Debug.Log("🔫 [DEBUG] KLIK! Geen munitie of kogel is niet chemisch behandeld.");
        }
    }

    void Schiet()
    {
        // Veiligheidscheck: hebben we alles ingevuld?
        if (kogelPrefab == null || schietPunt == null) 
        {
            Debug.LogError("🔴 [FOUT] KogelPrefab of SchietPunt mist in de Inspector!");
            return;
        }

        // 1. Speel Schiet Geluid af
        if (audioSource != null && schietGeluid != null) 
        {
            audioSource.PlayOneShot(schietGeluid);
        }

        // 2. Speel Particle Effect af (Muzzle Flash)
        if (schietParticles != null)
        {
            schietParticles.Play();
        }

        // 3. Spawn de kogel
        GameObject nieuweKogel = Instantiate(kogelPrefab, schietPunt.position, schietPunt.rotation);
        
        Rigidbody rb = nieuweKogel.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            rb.linearVelocity = schietPunt.forward * schietKracht;
            Debug.Log("🔫 [DEBUG] PANG! Kogel afgevuurd.");
        }
    }

    void OnTriggerEnter(Collider anderObject)
    {
        // Check of het munitie is en of we nog niet geladen zijn
        if (anderObject.CompareTag("Munitie") && !isGeladen)
        {
            // Zoek naar het script dat de kleurverandering bijhoudt
            BulletTransformation status = anderObject.GetComponent<BulletTransformation>();
            
            if (status != null && status.heeftEindstof)
            {
                isGeladen = true;
                Debug.Log("🔫 [DEBUG] Geweer geladen met actieve kogel!");
                
                // --- NIEUW: SPEEL HET LAADGELUID AF! ---
                if (audioSource != null && laadGeluid != null)
                {
                    audioSource.PlayOneShot(laadGeluid);
                }

                Destroy(anderObject.gameObject); 
            }
            else if (status != null && !status.heeftEindstof)
            {
                Debug.Log("⚠️ [DEBUG] Deze kogel heeft nog geen Eindstof geraakt!");
            }
        }
    }
}