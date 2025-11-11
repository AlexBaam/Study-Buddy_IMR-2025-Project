using UnityEngine;

public class StickyNoteCollisionTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[StickyNote] Collision with: {collision.gameObject.name}, tag: {collision.gameObject.tag}");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log($"[StickyNote] Still touching: {collision.gameObject.name}, tag: {collision.gameObject.tag}");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log($"[StickyNote] Stopped touching: {collision.gameObject.name}, tag: {collision.gameObject.tag}");
    }
}