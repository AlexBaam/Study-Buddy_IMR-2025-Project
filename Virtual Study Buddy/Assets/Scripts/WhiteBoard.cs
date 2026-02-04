using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Whiteboard : MonoBehaviour
{
    public int textureWidth = 2048;
    public int textureHeight = 2048;
    public Color clearColor = Color.white;

    public Color brushColor = Color.black;
    public int brushSize = 24;

    public Material targetMaterial;

    Texture2D drawTexture;
    Color[] brushCache;
    bool dirty = false;

    void Awake()
    {
        drawTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        drawTexture.wrapMode = TextureWrapMode.Clamp;

        ClearTexture();

        if (targetMaterial == null)
            targetMaterial = GetComponent<MeshRenderer>().material;

        targetMaterial.mainTexture = drawTexture;

        PrepareBrush();
    }

    void LateUpdate()
    {
        if (dirty)
        {
            drawTexture.Apply();
            dirty = false;
        }
    }

    public void ClearTexture()
    {
        Color[] fill = new Color[textureWidth * textureHeight];
        for (int i = 0; i < fill.Length; i++)
            fill[i] = clearColor;

        drawTexture.SetPixels(fill);
        drawTexture.Apply();
    }

    public void PrepareBrush()
    {
        int d = brushSize * 2 + 1;
        brushCache = new Color[d * d];

        for (int y = 0; y < d; y++)
        {
            for (int x = 0; x < d; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(brushSize, brushSize));
                float a = dist <= brushSize ? 1f : 0f;
                brushCache[y * d + x] = new Color(brushColor.r, brushColor.g, brushColor.b, a);
            }
        }
    }

    public void PaintAtUV(Vector2 uv)
    {
        int px = (int)(uv.x * textureWidth);
        int py = (int)(uv.y * textureHeight);

        int d = brushSize * 2 + 1;

        for (int y = 0; y < d; y++)
        {
            int ty = py + y - brushSize;
            if (ty < 0 || ty >= textureHeight) continue;

            for (int x = 0; x < d; x++)
            {
                int tx = px + x - brushSize;
                if (tx < 0 || tx >= textureWidth) continue;

                if (brushCache[y * d + x].a > 0)
                    drawTexture.SetPixel(tx, ty, brushColor);
            }
        }

        dirty = true;
    }
}