using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StickyNoteConnectable : MonoBehaviour
{
    StickyNote note;
    XRBaseInteractable interactable;
    bool isHovered;

    [Header("XR")]
    public InputActionReference triggerAction;

    bool triggerWasPressed;

    void Awake()
    {
        note = GetComponent<StickyNote>();
        interactable = GetComponent<XRBaseInteractable>();

        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    void OnEnable()
    {
        if (triggerAction != null)
            triggerAction.action.Enable();
    }

    void OnDisable()
    {
        if (triggerAction != null)
            triggerAction.action.Disable();
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

        bool keyboardPressed = Input.GetKeyDown(KeyCode.E);

        bool triggerPressed = false;

        if (triggerAction != null)
        {
            float triggerValue = triggerAction.action.ReadValue<float>();
            bool isPressedNow = triggerValue > 0.1f;

            triggerPressed = isPressedNow && !triggerWasPressed;
            triggerWasPressed = isPressedNow;
        }

        if (keyboardPressed || triggerPressed)
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
        triggerWasPressed = false;
    }
}
