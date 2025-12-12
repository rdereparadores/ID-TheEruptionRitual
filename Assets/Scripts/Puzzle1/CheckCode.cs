using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CheckCode : MonoBehaviour
{
    public Material correctMaterial;
    public AudioClip correctSound;
    private Renderer _lightRenderer;
    private AudioSource _audioSource;

    public PadWheelRotate wheel1;
    public PadWheelRotate wheel2;
    public PadWheelRotate wheel3;
    public PadWheelRotate wheel4;
    public XRGrabInteractable doorHandleInteractable;
    public Rigidbody doorRigidbody;

    private bool _solved = false;

    private void Awake()
    {
        _lightRenderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private bool IsCodeValid()
    {
        return
            wheel1.position == PadWheelRotate.WheelPosition.STAR &&
            wheel2.position == PadWheelRotate.WheelPosition.CIRCLE &&
            wheel3.position == PadWheelRotate.WheelPosition.PENTAGON &&
            wheel4.position == PadWheelRotate.WheelPosition.TRIANGLE;
    }

    private void Update()
    {
        if (_solved) return;
        
        if (IsCodeValid())
        {
            _lightRenderer.material = correctMaterial;
            _solved = true;
            doorRigidbody.isKinematic = false;
            doorHandleInteractable.enabled = true;
            _audioSource.PlayOneShot(correctSound);

            PlayerDialoguesHandler.Singleton.playPuzzle2StartRpc();
        }
    }
}
