using UnityEngine;

public class MonsterHit : MonoBehaviour
{
    public AudioClip hitmarkerSound; 
    private AudioSource monsterVoice; 

    void Start()
    {
        monsterVoice = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Munitie"))
        {
            // 2. De Hitmarker (we spelen hem af op de kogel, maar in 2D modus)
            if (hitmarkerSound != null)
            {
                // We gebruiken de statische PlayClipAtPoint, 
                // maar we zetten hem op de plek van de kogel.
                // OMDAT de kogel dichtbij het monster is, hoor je hem daar.
                AudioSource.PlayClipAtPoint(hitmarkerSound, collision.transform.position, 1.0f);
            }

            Destroy(collision.gameObject);
        }
    }
}