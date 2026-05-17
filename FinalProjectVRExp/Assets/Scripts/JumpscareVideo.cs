using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement; // DIT IS NIEUW: Nodig om scenes te laden!

public class JumpscareVideo : MonoBehaviour
{
    [Header("Video Instellingen")]
    public GameObject videoScherm;
    public VideoPlayer videoPlayer;
    public float duurCutscene = 3.0f;
    public AudioSource VideoAudio;

    [Header("Koppeling met Script 1")]
    [Tooltip("Sleep hier het JumpscareScript component in")]
    public JumpscareScript extraJumpscare;

    [Tooltip("Na hoeveel seconden video moet het plaatje in beeld knallen?")]
    public float vertragingVoorBeeld = 2.5f;

    [Header("Scene Overgang")]
    [Tooltip("Vul hier de exacte naam in van de scene waar je naartoe wilt (bijv. GameOver)")]
    public string sceneOmTeLaden = "GameOverScene";

    private bool isGeraakt = false;

    public QuestManager questDeath;

    void OnCollisionEnter(Collision collision)
    {
        CheckMonsterHit(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        CheckMonsterHit(other.gameObject);
    }

    void CheckMonsterHit(GameObject anderObject)
    {

        if (!enabled) return;

        if (anderObject.CompareTag("Seeker") && !isGeraakt)
        {
            questDeath.ResetAllQuests();
            Destroy(anderObject);
            StartCoroutine(SpeelVideoAf());
            if (VideoAudio != null) VideoAudio.Play();
        }
    }

    IEnumerator SpeelVideoAf()
    {
        isGeraakt = true;

        Debug.Log("🎬 Video Start!");

        // 1. Start de video
        if (videoScherm != null) videoScherm.SetActive(true);
        if (videoPlayer != null) videoPlayer.Play();

        // 2. Wacht het aantal seconden dat je hebt ingesteld
        yield return new WaitForSeconds(vertragingVoorBeeld);

        // 3. SEINTJE GEVEN! Roep het andere script aan om het plaatje te tonen
        if (extraJumpscare != null)
        {
            extraJumpscare.StartDeJumpscare();
        }

        // 4. Wacht de resterende tijd van de video af
        float resterendeTijd = duurCutscene - vertragingVoorBeeld;
        if (resterendeTijd > 0)
        {
            yield return new WaitForSeconds(resterendeTijd);
        }

        // 5. Zet de video uit en ga naar de nieuwe scene!
        if (videoScherm != null) videoScherm.SetActive(false);
        if (videoPlayer != null) videoPlayer.Stop();

        Debug.Log("💀 Jumpscare klaar, we laden scene: " + sceneOmTeLaden);
        SceneManager.LoadScene(sceneOmTeLaden);
    }
}