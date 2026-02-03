using UnityEngine;

public class WhiteboardColorManager : MonoBehaviour
{
    public static WhiteboardColorManager Instance;

    public Color currentColor = Color.black;

    void Awake()
    {
        Instance = this;
    }

    public void SetColor(Color c)
    {
        currentColor = c;
    }
}
