using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ColorCubeXR : MonoBehaviour
{
    Vector3 startPos;
    Quaternion startRot;

    XRGrabInteractable grab;
    Renderer rend;

    void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        grab = GetComponent<XRGrabInteractable>();
        rend = GetComponent<Renderer>();

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        WhiteboardColorManager.Instance.SetColor(rend.material.color);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
