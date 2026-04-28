using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimatie : MonoBehaviour
{
    [Tooltip("Koppel hier de actie voor de wijsvinger (Trigger)")]
    public InputActionProperty wijsvingerActie;
    
    [Tooltip("Koppel hier de actie voor de overige vingers (Grip)")]
    public InputActionProperty gripActie;
    
    [Tooltip("Sleep de Animator van de hand hier in")]
    public Animator handAnimator;

    void Update()
    {
        // Lees in hoe hard de speler de knoppen indrukt (tussen 0.0 en 1.0)
        float wijsvingerWaarde = wijsvingerActie.action.ReadValue<float>();
        float gripWaarde = gripActie.action.ReadValue<float>();

        // Stuur deze getallen naar de Animator
        if (handAnimator != null)
        {
            handAnimator.SetFloat("Trigger", wijsvingerWaarde);
            handAnimator.SetFloat("Grip", gripWaarde);
        }
    }
}