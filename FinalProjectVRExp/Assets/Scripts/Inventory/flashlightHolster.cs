using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HolsterableLight : MonoBehaviour
{
    [Header("References")]
    public Transform holsterPoint;
    public Rigidbody rb;
    public XRGrabInteractable grabInteractable;
    public KeyCardSpawner KCSpawn;
    public GameObject monster;
    public SeekerSpawnManager spawner;

    [Header("Holster Pose")]
    public Vector3 holsterLocalPosition = Vector3.zero;
    public Vector3 holsterLocalRotation = Vector3.zero;
    private bool FirstPickup = false;

    public QuestData Quest;
    public QuestManager manager;


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
        if (FirstPickup == false)
        {
            KCSpawn.Spawn();
            spawner.Spawn();
            Quest.isCompleted = true;
            manager.CheckAllQuests();
            FirstPickup = true;
        }
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
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
}