using System.Collections.Generic;
using UnityEngine;
public class lightManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> lights = new List<GameObject>();
    public AudioSource electricalSound;

    public void AllTurnOff()
    {
        foreach (GameObject light in lights)
        {
            if (light != null)
                light.SetActive(false);
        }
        electricalSound.PlayOneShot(electricalSound.clip);
    }

    public void AllTurnOn()
    {
        foreach (GameObject light in lights)
        {
            if (light != null)
                light.SetActive(true);
        }
    }
}