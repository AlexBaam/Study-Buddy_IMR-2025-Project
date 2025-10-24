using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BBasketball : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 releasePosition;
    private Rigidbody[] rigidbodies;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable[] grabInteractables;

    public Vector3 ReleasePosition => releasePosition;

    private void Start()
    {
        startPosition = transform.position;
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        grabInteractables = GetComponentsInChildren<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        foreach (var grab in grabInteractables)
        {
            grab.selectExited.AddListener(OnRelease);
        }
    }

    private void OnDestroy()
    {
        foreach (var grab in grabInteractables)
        {
            grab.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (args.interactableObject != null)
        {
            releasePosition = args.interactableObject.transform.position;
            Debug.Log("[BBasketball] Ball released at: " + releasePosition);
        }
    }

    public void ResetPosition()
    {
        foreach (var rb in rigidbodies)
        {
            if (!rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        transform.position = startPosition;
        Debug.Log("[BBasketball] Ball reset to start position.");
    }
}
