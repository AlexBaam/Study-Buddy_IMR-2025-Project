using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(AudioSource))]
public class RadioController : MonoBehaviour
{
    public AudioClip[] songs;
    private AudioSource audioSource;
    private XRGrabInteractable grab;
    private int currentSong = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        grab = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        // (optional) grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        // (optional) grab.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        var interactorObj = args.interactorObject.transform.gameObject;

        if (interactorObj.CompareTag("LeftHand"))
        {
            ChangeToNextSong();
        }
        else if (interactorObj.CompareTag("RightHand"))
        {
            TogglePlayPause();
        }
    }

    private void ChangeToNextSong()
    {
        if (songs == null || songs.Length == 0) return;

        currentSong = (currentSong + 1) % songs.Length;
        audioSource.clip = songs[currentSong];
        audioSource.Play();
    }

    private void TogglePlayPause()
    {
        if (audioSource.clip == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Pause(); 
        }
        else
        {
            audioSource.Play();
        }
    }

}
