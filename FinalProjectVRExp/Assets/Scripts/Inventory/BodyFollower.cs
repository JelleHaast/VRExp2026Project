using UnityEngine;

public class BodyFollower : MonoBehaviour
{
    public Transform targetCamera;
    public float heightOffset = -0.5f; // Adjust to move inventory to chest/waist level

    void Update()
    {
        // 1. Follow the camera's position
        Vector3 newPos = targetCamera.position;
        newPos.y += heightOffset; 
        transform.position = newPos;

        // 2. Follow ONLY the Y-axis rotation (the horizontal turn)
        Vector3 euler = targetCamera.eulerAngles;
        transform.rotation = Quaternion.Euler(0, euler.y, 0);
    }
}