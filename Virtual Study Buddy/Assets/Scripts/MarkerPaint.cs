using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private int penSize = 5;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;
    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;

    void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
        _tipHeight = tip.localScale.y;
    }

    void Update()
    {
        Draw();
    }

    void Draw()
    {
        Vector3 rayDir = -_touch.normal;

        if (Physics.Raycast(tip.position, tip.forward, out _touch, _tipHeight))
        {
            if (!_touch.transform.CompareTag("Whiteboard"))
            {
                _touchedLastFrame = false;
                return;
            }

            if (_whiteboard == null)
                _whiteboard = _touch.transform.GetComponent<Whiteboard>();

            Vector2 uv = _touch.textureCoord;
            int x = (int)(uv.x * _whiteboard.textureWidth);
            int y = (int)(uv.y * _whiteboard.textureHeight);

            if (_touchedLastFrame)
            {
                for (float t = 0.01f; t < 1.0f; t += 0.01f)
                {
                    int lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, t);
                    int lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, t);
                    _whiteboard.texture.SetPixels(lerpX, lerpY, penSize, penSize, _colors);
                }
            }

            _whiteboard.texture.SetPixels(x, y, penSize, penSize, _colors);
            _whiteboard.texture.Apply();

            _lastTouchPos = new Vector2(x, y);
            _touchedLastFrame = true;
        }
        else
        {
            _touchedLastFrame = false;
        }
        Debug.DrawRay(tip.position, tip.forward * _tipHeight, Color.red);
    }

}