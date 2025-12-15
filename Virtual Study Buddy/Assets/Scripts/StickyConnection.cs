using UnityEngine;

public class StickyConnection : MonoBehaviour
{
    public StickyNote a;
    public StickyNote b;

    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (a == null || b == null)
        {
            Destroy(gameObject);
            return;
        }

        lr.SetPosition(0, a.connectionPoint.position);
        lr.SetPosition(1, b.connectionPoint.position);
    }
}
