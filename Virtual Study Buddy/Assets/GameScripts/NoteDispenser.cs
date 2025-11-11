using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class StickyNoteDispenser : MonoBehaviour
{
    [Header("Prefab de Sticky Note")]
    public GameObject stickyNotePrefab;

    [Header("Locul de spawn (optional)")]
    public Transform spawnPoint;

    private XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        if (stickyNotePrefab == null) return;

        GameObject newNote = Instantiate(stickyNotePrefab);

        var interactor = args.interactorObject;

        var manager = args.manager;

        if (manager != null && interactor is IXRSelectInteractor selectInteractor)
        {
            var grabInteractable = newNote.GetComponent<IXRSelectInteractable>();
            if (grabInteractable != null)
            {
                manager.SelectEnter(selectInteractor, grabInteractable);
                return;
            }
        }

        newNote.transform.position = spawnPoint
            ? spawnPoint.position
            : transform.position + transform.forward * 0.2f;
        newNote.transform.rotation = Quaternion.LookRotation(-transform.forward);
    }
}
