using UnityEngine;

public class ObjectEmission : MonoBehaviour
{
    public Transform spelerHoofd;
    private Outline outlineScript;

    [Header("Instellingen")]
    public float activeerAfstand = 2.0f;
    public float outlineDikte = 4f; // Hoe dik de lijn wordt als je dichtbij bent

    void Start()
    {
        // Pak het Outline script veilig vast
        outlineScript = GetComponent<Outline>();
        if (outlineScript == null)
        {
            Debug.LogWarning("Let op: " + gameObject.name + " mist het 'Outline' component!");
        }

        // Noodoplossing: als je de camera bent vergeten in te slepen, zoek hem zelf!
        if (spelerHoofd == null && Camera.main != null)
        {
            spelerHoofd = Camera.main.transform;
        }
    }

    void Update()
    {
        // Veiligheidscheck: stop als de camera of de outline-tool mist
        if (spelerHoofd == null || outlineScript == null) return;

        // Meet de afstand
        float afstand = Vector3.Distance(transform.position, spelerHoofd.position);

        if (afstand <= activeerAfstand)
        {
            // Dichtbij: Zet de outline dikte op jouw gekozen waarde
            outlineScript.OutlineWidth = outlineDikte; 
        }
        else
        {
            // Ver weg: Knijp de lijn helemaal plat (0)
            outlineScript.OutlineWidth = 0f; 
        }
    }
}