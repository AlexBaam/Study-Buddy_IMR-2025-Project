using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WhiteboardScreenshotViewer : MonoBehaviour
{
    public MeshRenderer screenRenderer;

    [Header("Max size of the screen (world units)")]
    public Vector2 maxSize = new Vector2(1.2f, 0.8f);

    private List<Texture2D> screenshots = new List<Texture2D>();
    private int currentIndex = 0;

    private string screenshotsDir;
    private int lastFileCount = 0;

    void Start()
    {
        string desktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        screenshotsDir = Path.Combine(desktop, "WhiteboardScreenshots");

        LoadScreenshots(force: true);

        if (screenshots.Count > 0)
            ShowScreenshot(0);
        else
            ClearScreen();
    }

    void LoadScreenshots(bool force = false)
    {
        if (!Directory.Exists(screenshotsDir))
            return;

        string[] files = Directory.GetFiles(screenshotsDir, "*.png");

        if (!force && files.Length == lastFileCount)
            return; // nimic nou

        screenshots.Clear();

        foreach (string file in files)
        {
            byte[] data = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
            tex.LoadImage(data);
            screenshots.Add(tex);
        }

        lastFileCount = files.Length;

        if (currentIndex >= screenshots.Count)
            currentIndex = screenshots.Count - 1;
    }

    void ShowScreenshot(int index)
    {
        if (screenshots.Count == 0) return;

        index = Mathf.Clamp(index, 0, screenshots.Count - 1);
        currentIndex = index;

        Texture2D tex = screenshots[currentIndex];
        screenRenderer.material.mainTexture = tex;

        ResizeToFit(tex);
    }

    void ResizeToFit(Texture2D tex)
    {
        float imageAspect = (float)tex.width / tex.height;
        float targetAspect = maxSize.x / maxSize.y;

        float width, height;

        if (imageAspect > targetAspect)
        {
            width = maxSize.x;
            height = maxSize.x / imageAspect;
        }
        else
        {
            height = maxSize.y;
            width = maxSize.y * imageAspect;
        }

        transform.localScale = new Vector3(width, height, 1f);
    }

    void ClearScreen()
    {
        screenRenderer.material.mainTexture = null;
    }

    public void Next()
    {
        LoadScreenshots();
        if (screenshots.Count == 0) return;

        currentIndex++;
        if (currentIndex >= screenshots.Count)
            currentIndex = 0;

        ShowScreenshot(currentIndex);
    }

    public void Previous()
    {
        LoadScreenshots();
        if (screenshots.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
            currentIndex = screenshots.Count - 1;

        ShowScreenshot(currentIndex);
    }
}