using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class XRWhiteboardRayDrawer3X : MonoBehaviour
{
    [Header("Ray")]
    public Transform rayOrigin;
    public float maxDistance = 2f;
    public LayerMask whiteboardLayer;

    [Header("XR Input (optional)")]
    public XRInputDeviceButtonReader drawButton;
    [Range(0f, 1f)]
    public float pressThreshold = 0.1f;

    [Header("Mock / Keyboard")]
    public KeyCode keyboardKey = KeyCode.E;

    void Update()
    {
        bool drawFromTrigger =
            drawButton != null && drawButton.ReadValue() >= pressThreshold;

        bool drawFromKeyboard =
            Input.GetKey(keyboardKey);

        if (!drawFromTrigger && !drawFromKeyboard)
            return;

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, whiteboardLayer))
        {
            Whiteboard board = hit.collider.GetComponentInParent<Whiteboard>();
            if (board != null)
            {
                board.PaintAtUV(hit.textureCoord);
            }
        }
    }
}
