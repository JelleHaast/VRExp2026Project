using UnityEngine;
using NavKeypad;

public class KeypadButtonVR : MonoBehaviour
{
    [SerializeField] private KeypadButton keypadButton;

    private bool onCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Button pressed");
        if (onCooldown) return;

        if (other.CompareTag("VRFinger"))
        {
            Debug.Log("Button pressed");
            keypadButton.PressButton();
            onCooldown = true;
            //button debounce
            Invoke(nameof(ResetCooldown), 0.4f);
        }
    }

    private void ResetCooldown() => onCooldown = false;
}