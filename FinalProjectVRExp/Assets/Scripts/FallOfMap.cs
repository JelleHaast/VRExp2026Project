using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fallThreshold = -10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            transform.position = spawnPoint.position;
            if (rb != null) rb.linearVelocity = Vector3.zero;
        }
    }
}