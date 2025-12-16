using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CandleGrabNotifier : MonoBehaviour
{
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        CandlesManager.Instance.ToggleAllCandles();
    }
}