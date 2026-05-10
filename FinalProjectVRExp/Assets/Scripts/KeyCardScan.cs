using UnityEngine;
using System.Collections;
public class KeycardScan : MonoBehaviour
{
    public GameObject door;
    public string keyCardName;
    public float doorRiseHeight;
    public float doorRiseTime;
    private bool isUnlocked = false;

    void OnTriggerEnter(Collider other)
    {
        if (isUnlocked) return;

        if (other.CompareTag(keyCardName))
        {
            isUnlocked = true;
            Debug.Log("Access granted!");
            if (door != null)
                StartCoroutine(RaiseDoor());
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


            // Ease in: slow start, gets faster
            float eased = Mathf.SmoothStep(0f, 1f, t);

            door.transform.position = Vector3.Lerp(startPos, targetPos, eased);
            yield return null;
        }

        door.transform.position = targetPos;
    }
}