using UnityEngine;

public class WhiteboardColorButton : MonoBehaviour
{
    public Whiteboard whiteboard;
    public Color color;

    public void ApplyColor()
    {
        if (whiteboard == null) return;

        whiteboard.brushColor = color;
        whiteboard.PrepareBrush();
    }
}
