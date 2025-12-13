using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Whiteboard : MonoBehaviour
{
    public int textureWidth = 2048;
    public int textureHeight = 2048;
    public Color clearColor = Color.white;
    public int penSize = 5;

    [HideInInspector] public Texture2D texture;

    private Color[] _clearPixels;

    void Start()
    {
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        texture.wrapMode = TextureWrapMode.Clamp;

        _clearPixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < _clearPixels.Length; i++)
            _clearPixels[i] = clearColor;

        texture.SetPixels(_clearPixels);
        texture.Apply();

        GetComponent<Renderer>().material.mainTexture = texture;
    }

    public void Draw(Vector2 uv, Color[] colors, int penSize)
    {
        int x = (int)(uv.x * textureWidth - penSize / 2);
        int y = (int)(uv.y * textureHeight - penSize / 2);

        if (x < 0 || x >= textureWidth || y < 0 || y >= textureHeight)
            return;

        texture.SetPixels(x, y, penSize, penSize, colors);
        texture.Apply();
    }
}
