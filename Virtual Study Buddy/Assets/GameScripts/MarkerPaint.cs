using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class MarkerPaint : MonoBehaviour
{
    [Header("References")]
    public Whiteboard targetWhiteboard;
    [Tooltip("Transform that reprezintă vârful markerului (punctul de contact). Dacă e null, se folosește transformul curent.")]
    public Transform tipTransform;

    [Header("Paint settings")]
    [Tooltip("Distanța maximă de raycast la care vom detecta whiteboard-ul")]
    public float rayDistance = 0.05f; // 5 cm
    [Tooltip("Dacă true: pictăm numai când markerul este prins (grabbed)")]
    public bool paintOnlyWhenGrabbed = true;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable xrInteractable;
    private bool isGrabbed = false;

    void Awake()
    {
        if (tipTransform == null) tipTransform = this.transform;
        xrInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        if (xrInteractable != null)
        {
            xrInteractable.selectEntered.AddListener(OnSelectEntered);
            xrInteractable.selectExited.AddListener(OnSelectExited);
        }
    }

    void OnDestroy()
    {
        if (xrInteractable != null)
        {
            xrInteractable.selectEntered.RemoveListener(OnSelectEntered);
            xrInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    void Update()
    {
        if (paintOnlyWhenGrabbed && !isGrabbed) return;
        if (targetWhiteboard == null) return;

        Vector3 origin = tipTransform.position;
        Vector3 direction = tipTransform.forward;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, rayDistance))
        {
            Whiteboard wb = hit.collider.GetComponent<Whiteboard>();
            if (wb == null && hit.collider.transform != null)
            {
                wb = hit.collider.transform.GetComponentInParent<Whiteboard>();
            }

            if (wb != null)
            {
                Vector2 uv = hit.textureCoord;
                wb.PaintAtUV(uv);
            }
        }
    }
}