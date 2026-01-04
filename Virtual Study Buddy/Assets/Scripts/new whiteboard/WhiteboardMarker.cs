using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;

    private RaycastHit _touch;
    private Whiteboarddd _whiteboard;

    private Vector2 _touchPos;
    private Vector2 _lastTouchPos;

    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;

    private void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
        _tipHeight = _tip.localScale.y;
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Physics.Raycast(_tip.position, -transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                    _whiteboard = _touch.transform.GetComponent<Whiteboarddd>();

                if (_whiteboard == null)
                    return;

                _touchPos = _touch.textureCoord;

                int texWidth = (int)_whiteboard.textureSize.x;
                int texHeight = (int)_whiteboard.textureSize.y;

                int x = (int)(_touchPos.x * texWidth - _penSize / 2);
                int y = (int)(_touchPos.y * texHeight - _penSize / 2);

                if (x < 0 || y < 0 || x >= texWidth || y >= texHeight)
                {
                    _touchedLastFrame = false;
                    return;
                }

                if (_touchedLastFrame)
                {
                    _whiteboard.texture.SetPixels(x, y, _penSize, _penSize, _colors);

                    for (float f = 0.01f; f < 1f; f += 0.03f)
                    {
                        int lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        int lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                    }

                    transform.rotation = _lastTouchRot;
                    _whiteboard.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }

        _whiteboard = null;
        _touchedLastFrame = false;
    }
}