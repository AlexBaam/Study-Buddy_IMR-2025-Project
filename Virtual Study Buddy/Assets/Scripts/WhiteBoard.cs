using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Whiteboard : MonoBehaviour
{
    [Header("Texture settings")]
    public int textureWidth = 2048;
    public int textureHeight = 2048;
    public Color clearColor = Color.white;

    [Header("Brush settings")]
    public Color brushColor = Color.black;
    [Tooltip("Brush radius in texture pixels")]
    public int brushSize = 24;

    [Header("References")]
    public Material targetMaterial;

    private Texture2D drawTexture;
    private MeshRenderer meshRenderer;
    private Color[] brushCache;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (targetMaterial == null)
        {
            targetMaterial = meshRenderer.sharedMaterial;
        }

        InitializeTexture();
    }

    void InitializeTexture()
    {
        drawTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        drawTexture.wrapMode = TextureWrapMode.Clamp;
        ClearTexture();

        if (targetMaterial != null)
        {
            targetMaterial.mainTexture = drawTexture;
        }

        PrepareBrushCache();
    }

    void ClearTexture()
    {
        Color[] fill = new Color[textureWidth * textureHeight];
        for (int i = 0; i < fill.Length; i++) fill[i] = clearColor;
        drawTexture.SetPixels(fill);
        drawTexture.Apply();
    }

    void PrepareBrushCache()
    {
        int diameter = brushSize * 2 + 1;
        brushCache = new Color[diameter * diameter];
        int center = brushSize;
        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                int dx = x - center;
                int dy = y - center;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = dist <= brushSize ? 1f : 0f;
                brushCache[y * diameter + x] = new Color(brushColor.r, brushColor.g, brushColor.b, alpha);
            }
        }
    }

    public void PaintAtUV(Vector2 uv)
    {
        if (drawTexture == null) return;

        int px = Mathf.RoundToInt(uv.x * (textureWidth - 1));
        int py = Mathf.RoundToInt(uv.y * (textureHeight - 1));

        int diameter = brushSize * 2 + 1;
        int startX = px - brushSize;
        int startY = py - brushSize;

        for (int by = 0; by < diameter; by++)
        {
            int y = startY + by;
            if (y < 0 || y >= textureHeight) continue;

            for (int bx = 0; bx < diameter; bx++)
            {
                int x = startX + bx;
                if (x < 0 || x >= textureWidth) continue;

                Color brushPixel = brushCache[by * diameter + bx];
                if (brushPixel.a <= 0f) continue;

                drawTexture.SetPixel(x, y, Color.Lerp(drawTexture.GetPixel(x, y), brushColor, brushPixel.a));
            }
        }

        drawTexture.Apply();
    }

    public void SetBrushColor(Color c)
    {
        brushColor = c;
        PrepareBrushCache();
    }

    public byte[] EncodeToPNG()
    {
        if (drawTexture == null) return null;
        return drawTexture.EncodeToPNG();
    }
}
