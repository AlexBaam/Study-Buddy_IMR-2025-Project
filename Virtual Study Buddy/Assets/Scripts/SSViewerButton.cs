using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRViewerButton : MonoBehaviour
{
    public enum ButtonType { Next, Previous }
    public ButtonType buttonType;

    public WhiteboardScreenshotViewer viewer;

    XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnPress);
    }

    void OnPress(SelectEnterEventArgs args)
    {
        if (viewer == null) return;

        if (buttonType == ButtonType.Next)
            viewer.Next();
        else
            viewer.Previous();
    }
}
