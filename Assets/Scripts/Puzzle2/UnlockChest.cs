using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class UnlockChest : MonoBehaviour
{
    public GameObject key;
    public XRGrabInteractable chest;
    
    private Rigidbody _rigidbody;
    private bool _firstTime = true;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == key.name)
        {
            _rigidbody.isKinematic = false;
            chest.enabled = true;

            if (_firstTime)
            {
                PlayerDialoguesHandler.Singleton.playPuzzle2OnChestUnlock();
                _firstTime = false;
            }
        }
    }
}
