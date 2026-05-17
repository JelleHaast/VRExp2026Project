using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

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

    public QuestData Quest;
    public QuestManager manager;


    private bool isHeld = false;

    //subscribe to the Listener
    void OnEnable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);   // remove first
        grabInteractable.selectEntered.AddListener(OnGrabbed);      // then add once
        grabInteractable.selectExited.RemoveListener(OnReleased);
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
        Debug.Log($"[HolsterableLight] Grabbed — Quest.isCompleted={Quest.isCompleted}, instanceID={GetInstanceID()}");

        // Use QuestData as the single source of truth instead of static bool
        if (!Quest.isCompleted)
        {
            KCSpawn.Spawn();
            spawner.Spawn();
            Quest.isCompleted = true;
            manager.CheckAllQuests();
        }

        isHeld = true;
        transform.SetParent(null);
        rb.isKinematic = false;
        holsterPoint.rotation = Quaternion.Euler(0, holsterPoint.parent.eulerAngles.y, 0);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
        StartCoroutine(ReturnToHolsterNextFrame());
    }

    IEnumerator ReturnToHolsterNextFrame()
    {
        yield return null; // wait one frame for XR to finish releasing
        ReturnToHolster();
    }



    void ReturnToHolster()
    {
        if (isHeld) return; // safety check

        transform.SetParent(holsterPoint);
        transform.localPosition = holsterLocalPosition;
        transform.localRotation = Quaternion.Euler(holsterLocalRotation);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
}