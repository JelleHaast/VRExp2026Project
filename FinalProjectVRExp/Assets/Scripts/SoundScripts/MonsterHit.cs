using UnityEngine;

public class MonsterHit : MonoBehaviour
{
    [Header("Monster Status")]
    public int gezondheid = 3;
    private bool isDood = false;

    [Header("Componenten")]
    private AudioSource monsterVoice;
    private Animator animator; // Verwijzing naar de animator

    [Header("Geluiden")]
    public AudioClip hitmarkerSound;

    public QuestData Quest;
    public QuestManager manager;

    void Start()
    {
        monsterVoice = GetComponent<AudioSource>();
        animator = GetComponent<Animator>(); // Pak de animator op het monster
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDood) return;

        if (collision.gameObject.CompareTag("Munitie"))
        {
            MonsterRaken();
            Destroy(collision.gameObject);
        }
    }

    void MonsterRaken()
    {
        gezondheid--;

        // Speel geluiden
        if (monsterVoice != null) monsterVoice.PlayOneShot(monsterVoice.clip);
        if (hitmarkerSound != null)
        {
            AudioSource.PlayClipAtPoint(hitmarkerSound, Camera.main.transform.position, 1f);
        }

        // Optioneel: speel een 'get hit' animatie als je die hebt
        // animator.SetTrigger("GetHit");

        if (gezondheid <= 0)
        {
            MonsterDood();
        }
    }

    void MonsterDood()
    {
        isDood = true;
        Debug.Log("💀 Animatie start: Monster gaat dood!");

        // Activeer de trigger in de Animator
        if (animator != null)
        {
            animator.SetTrigger("death1"); // Zorg dat deze naam exact hetzelfde is in je Animator
        }

        // Zet de collider uit zodat je het lijk niet meer kunt raken
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Verwijder het object pas NA de animatie (bijv. na 5 seconden)
        Destroy(gameObject, 5f);
        Quest.isCompleted = true;
        manager.CheckAllQuests();
    }
}