using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ColorPickup : MonoBehaviour
{
    public Whiteboard whiteboard;
    public Color color = Color.black;

    XRGrabInteractable grab;
    Rigidbody rb;

    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (whiteboard == null) return;

        whiteboard.brushColor = color;
        whiteboard.PrepareBrush();
    }

    void OnReleased(SelectExitEventArgs args)
    {
        grab.enabled = false;

        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = startPosition;
        rb.rotation = startRotation;

        rb.isKinematic = false;

        grab.enabled = true;
    }
}
