using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WhiteboardClearButtonXR : MonoBehaviour
{
    public Transform pressPart;
    public float pressDepth = 0.015f;
    public Whiteboard whiteboard;

    Vector3 initialLocalPos;
    XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        initialLocalPos = pressPart.localPosition;

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        pressPart.localPosition = initialLocalPos - new Vector3(0, pressDepth, 0);

        if (whiteboard != null)
        {
            whiteboard.ClearTexture();
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        pressPart.localPosition = initialLocalPos;
    }
}
