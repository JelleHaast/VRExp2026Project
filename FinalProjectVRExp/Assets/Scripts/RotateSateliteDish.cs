using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float targetAngle = 240f;
    [SerializeField] private AudioSource audioSource;

    public QuestData Quest;
    public QuestManager manager;

    private float rotated = 0f;
    private bool rotating = false;

    public void StartRotation()
    {
        if (!rotating)
            StartCoroutine(Rotate());
    }

    private System.Collections.IEnumerator Rotate()
    {
        rotating = true;
        rotated = 0f;
        audioSource.Play();

        while (rotated < targetAngle)
        {
            float step = rotationSpeed * Time.deltaTime;
            step = Mathf.Min(step, targetAngle - rotated);
            transform.Rotate(0f, step, 0f);
            rotated += step;
            yield return null;
        }

        audioSource.Stop();

        rotating = false;

        Quest.isCompleted = true;
        manager.CheckAllQuests();
    }
}