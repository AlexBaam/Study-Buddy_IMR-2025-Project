using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class StickyNoteDispenser : MonoBehaviour
{
    [Header("Sticky Note Settings")]
    public GameObject stickyNotePrefab;
    public Transform spawnPoint;

    private XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnDispenserUsed);
    }

    void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnDispenserUsed);
    }

    private void OnDispenserUsed(SelectEnterEventArgs args)
    {
        if (stickyNotePrefab == null)
        {
            Debug.LogError("[StickyNoteDispenser] Missing stickyNotePrefab!", this);
            return;
        }

        GameObject note = Instantiate(stickyNotePrefab);

        Vector3 spawnPos = spawnPoint
            ? spawnPoint.position
            : transform.position + transform.up * 0.1f + transform.forward * 0.05f;

        note.transform.position = spawnPos;

        var cam = Camera.main;
        if (cam != null)
        {
            Vector3 lookDir = (cam.transform.position - note.transform.position).normalized;
            note.transform.rotation = Quaternion.LookRotation(lookDir);
        }

        TryAutoGrab(args, note);
    }

    private void TryAutoGrab(SelectEnterEventArgs args, GameObject note)
    {
        var manager = args.manager;
        var interactor = args.interactorObject as IXRSelectInteractor;
        var interactable = note.GetComponent<IXRSelectInteractable>();

        if (manager != null && interactor != null && interactable != null)
        {
            manager.SelectEnter(interactor, interactable);
        }
    }
}
