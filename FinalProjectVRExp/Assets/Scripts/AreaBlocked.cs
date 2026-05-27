using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProximityFade : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI text;
    public float maxDistance = 5f;
    public float minDistance = 1f;

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        float alpha = 1f - Mathf.InverseLerp(minDistance, maxDistance, dist);

        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }
}