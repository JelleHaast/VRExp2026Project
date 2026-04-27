using System.Collections;
using UnityEngine;

public class AvatarAutoScaler : MonoBehaviour
{
    [Tooltip("Sleep hier je Main Camera (VR Bril) in")]
    public Transform vrCamera; 
    
    [Tooltip("Hoe lang is de 3D-astronaut standaard? (ongeveer 1.75 meter)")]
    public float avatarStandaardHoogte = 1.75f; 

    void Start()
    {
        // We wachten een halve seconde bij de start, zodat de VR-bril zeker weet waar hij is
        StartCoroutine(PasHoogteAan());
    }

    IEnumerator PasHoogteAan()
    {
        yield return new WaitForSeconds(0.5f);

        if (vrCamera != null)
        {
            // We meten hoe hoog jouw fysieke bril vanaf de vloer is (lokale Y-positie)
            float spelerHoogte = vrCamera.localPosition.y; 
            
            // Als de speler om de een of andere reden op de grond ligt, voorkomen we dat de avatar verdwijnt
            if (spelerHoogte < 0.5f) spelerHoogte = 0.5f;

            // Bereken de schaal-verhouding
            float schaal = spelerHoogte / avatarStandaardHoogte;

            // Pas de grootte van de astronaut aan in alle richtingen (X, Y en Z)
            transform.localScale = new Vector3(schaal, schaal, schaal);
        }
    }
}