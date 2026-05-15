using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScriptActivationEntry
{
    public GameObject targetObject;
    public string scriptName;
}

public class KeycardScan : MonoBehaviour
{
    public GameObject door;
    public string keyCardName;
    public float doorRiseHeight;
    public float doorRiseTime;
    private bool isUnlocked = false;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorSlide;
    [SerializeField] private AudioClip granted;
    [SerializeField] private AudioClip denied;

    [Header("Scripts to enable on door open")]
    public ScriptActivationEntry[] scriptsToEnable;

    void OnTriggerEnter(Collider other)
    {
        if (isUnlocked) return;

        if (other.CompareTag(keyCardName))
        {
            isUnlocked = true;
            if (door != null)
            {
                audioSource.PlayOneShot(granted);
                audioSource.PlayOneShot(doorSlide);
                StartCoroutine(RaiseDoor());
            }

            foreach (ScriptActivationEntry s in scriptsToEnable)
            {
                if (s.targetObject == null) continue;
                MonoBehaviour script = s.targetObject.GetComponent(s.scriptName) as MonoBehaviour;
                if (script != null)
                    script.enabled = true;
                else
                    Debug.LogWarning("Script " + s.scriptName + " not found on " + s.targetObject.name);
            }
        }
        else
        {
            audioSource.PlayOneShot(denied);
        }
    }

    IEnumerator RaiseDoor()
    {
        Vector3 startPos = door.transform.position;
        Vector3 targetPos = startPos + Vector3.up * doorRiseHeight;
        float elapsed = 0f;

        while (elapsed < doorRiseTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / doorRiseTime;
            float eased = Mathf.SmoothStep(0f, 1f, t);
            door.transform.position = Vector3.Lerp(startPos, targetPos, eased);
            yield return null;
        }

        door.transform.position = targetPos;
    }
}