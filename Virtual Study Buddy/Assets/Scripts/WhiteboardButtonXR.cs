using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WhiteboardButtonXR : MonoBehaviour
{
    public Transform pressPart;
    public float pressDepth = 0.015f;
    public Camera captureCamera;
    Vector3 initialLocalPos;
    XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        initialLocalPos = pressPart.localPosition;
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        pressPart.localPosition = initialLocalPos - new Vector3(0, pressDepth, 0);
        CaptureWhiteboard();
    }

    void OnRelease(SelectExitEventArgs args)
    {
        pressPart.localPosition = initialLocalPos;
    }

    void CaptureWhiteboard()
    {
        RenderTexture rt = captureCamera.targetTexture;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] data = tex.EncodeToPNG();

        string desktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string dir = Path.Combine(desktop, "WhiteboardScreenshots");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string path = Path.Combine(dir, "Whiteboard_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");
        File.WriteAllBytes(path, data);

        RenderTexture.active = null;
        Destroy(tex);
    }
}
