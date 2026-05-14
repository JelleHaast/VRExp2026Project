using UnityEngine;

public class FrameRateCap : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 72;
    }
}