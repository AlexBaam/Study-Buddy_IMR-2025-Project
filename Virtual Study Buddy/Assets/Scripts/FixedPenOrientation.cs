using UnityEngine;

public class MarkerSurfaceStop : MonoBehaviour
{
    public Transform tip;
    public LayerMask whiteboardLayer;
    public float stopDistance = 0.002f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(tip.position, tip.forward, out hit, 0.02f, whiteboardLayer))
        {
            Vector3 normal = hit.normal;

            // folosim NOUL API
            Vector3 v = rb.linearVelocity;

            // eliminăm componenta de viteză spre tablă
            Vector3 towardSurface = Vector3.Project(v, -normal);
            rb.linearVelocity = v - towardSurface;

            // poziționare stabilă pe suprafață
            transform.position = hit.point + normal * stopDistance;
        }
    }
}
