using UnityEngine;

public class PenAlign : MonoBehaviour
{
    public Transform pen;  // obiectul "pen"
    public Transform tip;  // vârful

    private bool isTouching = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Whiteboard"))
        {
            isTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Whiteboard"))
        {
            isTouching = false;
        }
    }

    void Update()
    {
        if (isTouching)
        {
            // Ray ca să detectăm normalul exact
            if (Physics.Raycast(tip.position, -tip.forward, out RaycastHit hit, 0.02f))
            {
                // face axa penului să fie perpendiculară pe suprafață
                Quaternion targetRot = Quaternion.FromToRotation(pen.up, hit.normal) * pen.rotation;

                // poți folosi și lerp pentru smooth:
                pen.rotation = Quaternion.Lerp(pen.rotation, targetRot, Time.deltaTime * 40f);
            }
        }
    }
}
