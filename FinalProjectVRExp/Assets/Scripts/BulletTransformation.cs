using UnityEngine;

public class BulletTransformation : MonoBehaviour
{
    public bool heeftEindstof = false;
    public Material geactiveerdMateriaal;

    // Deze functie wordt aangeroepen door de Collision module van het Particle System
    void OnParticleCollision(GameObject other)
    {
        if (!heeftEindstof)
        {
            Debug.Log("💥 [SUCCESS] Particle botsing met de kogel!");
            ActiveerKogel();
        }
    }

    // We laten deze erin als reserve
    void OnParticleTrigger()
    {
        if (!heeftEindstof)
        {
            Debug.Log("💧 [SUCCESS] Particle trigger contact!");
            ActiveerKogel();
        }
    }

    public void ActiveerKogel()
    {
        heeftEindstof = true;
        
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer == null) renderer = GetComponent<MeshRenderer>();

        if (renderer != null && geactiveerdMateriaal != null)
        {
            renderer.material = geactiveerdMateriaal;
        }
    }
}