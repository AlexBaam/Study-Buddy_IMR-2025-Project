using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StickyNote : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grab;

    public float stickDistance = 0.1f;
    public float offset = 0.002f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();

        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        TryStick();
    }

    private void TryStick()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, stickDistance))
        {
            if (hit.collider.CompareTag("Wall"))
                Stick(hit);
            return;
        }

        if (Physics.Raycast(transform.position, -transform.forward, out hit, stickDistance))
        {
            if (hit.collider.CompareTag("Wall"))
                Stick(hit);
        }
    }

    private void Stick(RaycastHit hit)
    {
        transform.position = hit.point + hit.normal * offset;
        transform.rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);

        rb.useGravity = false;
        rb.isKinematic = true;
    }
}
