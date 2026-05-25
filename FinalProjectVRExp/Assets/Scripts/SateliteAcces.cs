using UnityEngine;
using System.Collections;

public class SateliteAcces : MonoBehaviour
{
    public GameObject door;
    public QuestData quest;
    public float doorRiseHeight;
    public float doorRiseTime;

    public bool isUnlocked = false;

    void Update()
    {
        if (quest.isCompleted && !isUnlocked)
        {
            isUnlocked = true;
            StartCoroutine(RaiseDoor());
            enabled = false; // stop Update from running
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