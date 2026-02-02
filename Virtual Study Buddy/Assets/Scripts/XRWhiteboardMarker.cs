using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class XRWhiteboardMarker : MonoBehaviour
{
    public Transform tip;
    public float tipRadius = 0.004f;
    public float drawDistance = 0.01f;
    public float surfaceOffset = 0.001f;
    public LayerMask whiteboardLayer;

    XRGrabInteractable grab;
    Rigidbody rb;
    bool isGrabbed;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grab.selectEntered.AddListener(_ => isGrabbed = true);
        grab.selectExited.AddListener(_ => isGrabbed = false);
    }

    void FixedUpdate()
    {
        if (!isGrabbed) return;

        Vector3 dir = tip.forward;

        if (Physics.SphereCast(
            tip.position,
            tipRadius,
            dir,
            out RaycastHit hit,
            drawDistance,
            whiteboardLayer))
        {
            Whiteboard board = hit.collider.GetComponentInParent<Whiteboard>();
            if (board != null)
            {
                board.PaintAtUV(hit.textureCoord);
            }

            Vector3 correction = hit.point + hit.normal * surfaceOffset - tip.position;
            rb.position += correction;
        }
    }
}
