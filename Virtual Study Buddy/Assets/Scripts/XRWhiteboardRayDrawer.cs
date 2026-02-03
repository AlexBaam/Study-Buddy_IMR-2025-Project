using UnityEngine;

public class WhiteboardKeyboardDrawer : MonoBehaviour
{
    public KeyCode drawKey = KeyCode.E;
    public float maxDistance = 2f;
    public LayerMask whiteboardLayer;

    Vector2? lastUV = null;

    const float uvStep = 0.002f;
    const int maxSteps = 64;

    void Update()
    {
        if (!Input.GetKey(drawKey))
        {
            lastUV = null;
            return;
        }

        if (!Physics.Raycast(transform.position, transform.forward,
            out RaycastHit hit, maxDistance, whiteboardLayer))
        {
            lastUV = null;
            return;
        }

        Whiteboard board = hit.collider.GetComponentInParent<Whiteboard>();
        if (board == null) return;

        Vector2 uv = hit.textureCoord;

        if (lastUV.HasValue)
            DrawSmooth(board, lastUV.Value, uv);
        else
            board.PaintAtUV(uv);

        lastUV = uv;
    }

    void DrawSmooth(Whiteboard board, Vector2 from, Vector2 to)
    {
        float dist = Vector2.Distance(from, to);
        int steps = Mathf.Min(maxSteps, Mathf.CeilToInt(dist / uvStep));

        for (int i = 0; i <= steps; i++)
        {
            Vector2 uv = Vector2.Lerp(from, to, i / (float)steps);
            board.PaintAtUV(uv);
        }
    }
}
