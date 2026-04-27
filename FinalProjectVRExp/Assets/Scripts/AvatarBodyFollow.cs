using UnityEngine;

public class AvatarBodyFollow : MonoBehaviour
{
    [Tooltip("Sleep hier je Main Camera in")]
    public Transform vrCamera;

    private float startHoogte;

    void Start()
    {
        // Onthoud de hoogte (Y) die we net handmatig perfect hebben ingesteld
        startHoogte = transform.position.y; 
    }

    void Update()
    {
        if (vrCamera != null)
        {
            // 1. Zorg dat het lichaam met je meeloopt als je fysiek een stap zet (X en Z), 
            // maar blijf netjes op de vloer-hoogte (Y)
            transform.position = new Vector3(vrCamera.position.x, startHoogte, vrCamera.position.z);

            // 2. Kopieer alleen de links/rechts draaiing van je hoofd (Y-as).
            // (Hierdoor buigt het lichaam niet voorover als jij naar de vloer kijkt)
            transform.rotation = Quaternion.Euler(0, vrCamera.eulerAngles.y, 0);
        }
    }
}