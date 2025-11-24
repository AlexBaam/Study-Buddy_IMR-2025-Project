using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRSimpleInteractable))]
public class RadioDebug : MonoBehaviour
{
    private AudioSource audioSource;
    private XRSimpleInteractable interactable;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        interactable = GetComponent<XRSimpleInteractable>();
    }

    void Start()
    {
        Debug.Log("[RADIO] Start() a fost apelat");

        if (audioSource.clip == null)
            Debug.LogWarning("[RADIO] NU ai setat un AudioClip pe AudioSource!");
        else
            Debug.Log("[RADIO] AudioClip este setat: " + audioSource.clip.name);

        audioSource.loop = true;
        audioSource.Play();

        if (audioSource.isPlaying)
            Debug.Log("[RADIO] Muzica a pornit cu succes!");
        else
            Debug.LogError("[RADIO] Muzica NU rulează. Verifică AudioSource.");

        // Ascultăm evenimentul pentru select (funcționează și cu ray)
        interactable.selectEntered.AddListener(OnSelected);
        interactable.hoverEntered.AddListener(OnHovered);
    }

    private void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnSelected);
        interactable.hoverEntered.RemoveListener(OnHovered);
    }

    private void OnHovered(HoverEnterEventArgs args)
    {
        Debug.Log("[RADIO] Hover pe radio de către: " + args.interactorObject.transform.name);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        Debug.Log("[RADIO] Select radio de către: " + args.interactorObject.transform.name);

        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            Debug.Log("[RADIO] Muzica pusă pe PAUZĂ.");
        }
        else
        {
            audioSource.UnPause();
            Debug.Log("[RADIO] Muzica reluată.");
        }
    }
}
