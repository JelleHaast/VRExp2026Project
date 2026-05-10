using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HolsterableLight : MonoBehaviour
{
    [Header("References")]
    public Transform holsterPoint;
    public Rigidbody rb;
    public XRGrabInteractable grabInteractable;

    [Header("Holster Pose")]
    public Vector3 holsterLocalPosition = Vector3.zero;
    public Vector3 holsterLocalRotation = Vector3.zero;

    private bool isHeld = false;

    //subscribe to the Listener
    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    //unsubscribe for memory leaks
    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
        transform.SetParent(null);

        // Unfreeze so XR Toolkit can move it
        rb.isKinematic = false;
        holsterPoint.rotation = Quaternion.Euler(0, holsterPoint.parent.eulerAngles.y, 0);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        ReturnToHolster();
    }



    void ReturnToHolster()
    {
        transform.SetParent(holsterPoint);
        transform.localPosition = holsterLocalPosition;
        transform.localRotation = Quaternion.Euler(holsterLocalRotation);

        // Freeze it
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}