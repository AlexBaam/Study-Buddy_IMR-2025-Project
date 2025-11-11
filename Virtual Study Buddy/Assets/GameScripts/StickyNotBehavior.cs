using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public class StickyNoteBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    private bool isStuck = false;

    [Header("Sticky Settings")]
    public float stickDistance = 0.5f; // cât de departe caută perete
    public float offsetFromWall = 0.002f;
    public float tiltAngle = 15f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        isStuck = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        TryStickToWall();
    }

    private void TryStickToWall()
    {
        // raycast în față
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, stickDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                StickToSurface(hit);
                return;
            }
        }

        // raycast înapoi
        ray = new Ray(transform.position, -transform.forward);
        if (Physics.Raycast(ray, out hit, stickDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                StickToSurface(hit);
            }
        }
    }

    private void StickToSurface(RaycastHit hit)
    {
        // poziție + mic offset
        transform.position = hit.point + hit.normal * offsetFromWall;

        // orientare perpendiculară pe perete + tilt
        Quaternion baseRot = Quaternion.LookRotation(-hit.normal, Vector3.up);
        Quaternion tilt = Quaternion.AngleAxis(tiltAngle, transform.right);
        transform.rotation = baseRot * tilt;

        // dezactivează fizica
        rb.isKinematic = true;
        rb.useGravity = false;

        isStuck = true;
    }
}