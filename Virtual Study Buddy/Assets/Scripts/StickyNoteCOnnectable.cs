using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StickyNoteConnectable : MonoBehaviour
{
    StickyNote note;
    XRBaseInteractable interactable;

    void Awake()
    {
        note = GetComponent<StickyNote>();
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelect);
    }

    void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnSelect);
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        Transform t = args.interactorObject.transform;

        bool isLeft =
            t.CompareTag("LeftHand") ||
            (t.parent != null && t.parent.CompareTag("LeftHand"));

        if (!isLeft)
            return;

        StickyConnectionManager.Instance.SelectSticky(note);
    }
}
