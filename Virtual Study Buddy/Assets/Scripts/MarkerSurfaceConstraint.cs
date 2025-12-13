using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MarkerSurfaceConstraint : MonoBehaviour
{
    public Transform tip;
    public LayerMask whiteboardLayer;
    public float maxPenetration = 0.0015f;

    private XRGrabInteractable grab;
    private bool isGrabbed;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(_ => isGrabbed = true);
        grab.selectExited.AddListener(_ => isGrabbed = false);
    }

    void Update()
    {
        if (!isGrabbed)
            return;

        RaycastHit hit;
        if (Physics.Raycast(tip.position, tip.forward, out hit, 0.02f, whiteboardLayer))
        {
            Vector3 normal = hit.normal;

            // proiectăm poziția pe planul tablei
            float penetration = Vector3.Dot(
                hit.point - tip.position,
                normal
            );

            if (penetration < -maxPenetration)
            {
                // mutăm DOAR cât trebuie
                transform.position += normal * (-penetration - maxPenetration);
            }
        }
    }
}
