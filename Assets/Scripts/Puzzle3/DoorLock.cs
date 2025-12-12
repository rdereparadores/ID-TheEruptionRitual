using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorLock : MonoBehaviour
{
    public XRGrabInteractable doorInteractable;
    public Rigidbody doorRigidbody;
    public Material unlockedMaterial;
    public AudioClip unlockedSound;

    private Renderer _renderer;
    private AudioSource _audioSource;
    private bool _alreadyUnlocked = false;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Unlock()
    {
        if (!_alreadyUnlocked)
        {
            doorRigidbody.isKinematic = false;
            doorInteractable.enabled = true;
            _renderer.material = unlockedMaterial;
            _audioSource.PlayOneShot(unlockedSound);
            _alreadyUnlocked = true;
        }
    }
}
