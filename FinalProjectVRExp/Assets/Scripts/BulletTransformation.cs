using UnityEngine;

public class BulletTransformation : MonoBehaviour
{
    [Header("Status")]
    public bool heeftEindstof = false;

    [Header("Visuele Feedback")]
    public Material geactiveerdMateriaal;

    void OnTriggerEnter(Collider ander)
    {
        // Let op: 'Eindstof' moet met een hoofdletter E als dat je Tag is!
        if (ander.CompareTag("Eindstof") && !heeftEindstof)
        {
            ActiveerKogel();
        }
    }

    void ActiveerKogel()
    {
        heeftEindstof = true;
        Debug.Log("🧪 [DEBUG] Kogel verkleurt nu!");

        // We zoeken nu ook in de kinderen naar de MeshRenderer
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        
        if (renderer != null && geactiveerdMateriaal != null)
        {
            renderer.material = geactiveerdMateriaal;
        }
        else
        {
            Debug.LogWarning("⚠️ Renderer niet gevonden op kogel of kind-object!");
        }
    }
}