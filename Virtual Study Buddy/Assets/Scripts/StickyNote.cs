using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Sticky note behavior: grab, release, stick to walls, gravity, collision ignore.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Collider))]
public class StickyNote : MonoBehaviour
{
    [Header("Stick Settings")]
    public float stickDistance = 0.1f;
    public float stickOffset = 0.002f;

    [Header("Ignore Collisions While Grabbed")]
    public Collider[] ignoreWhileGrabbed;

    private Rigidbody rb;
    private XRGrabInteractable grab;
    private Collider myCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        myCollider = GetComponent<Collider>();

        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        foreach (var col in ignoreWhileGrabbed)
        {
            if (col != null)
                Physics.IgnoreCollision(myCollider, col, true);
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        foreach (var col in ignoreWhileGrabbed)
        {
            if (col != null)
                Physics.IgnoreCollision(myCollider, col, false);
        }

        TryStick();
    }

    private void TryStick()
    {
        bool stuck = false;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, stickDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Stick(hit);
                stuck = true;
            }
        }

        if (!stuck)
        {
            if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit2, stickDistance))
            {
                if (hit2.collider.CompareTag("Wall"))
                {
                    Stick(hit2);
                    stuck = true;
                }
            }
        }

        if (!stuck)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    private void Stick(RaycastHit hit)
    {
        transform.position = hit.point + hit.normal * stickOffset;

        transform.rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);

        rb.useGravity = false;
        rb.isKinematic = true;
    }
}
