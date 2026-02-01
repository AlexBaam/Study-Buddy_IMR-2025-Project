using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StickyNoteConnectable : MonoBehaviour
{
    StickyNote note;
    XRBaseInteractable interactable;
    bool isHovered;

    void Awake()
    {
        note = GetComponent<StickyNote>();
        interactable = GetComponent<XRBaseInteractable>();

        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    void OnDestroy()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    void Update()
    {
        if (!isHovered)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StickyConnectionManager.Instance.SelectSticky(note);
        }
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        isHovered = true;
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        isHovered = false;
    }
}
