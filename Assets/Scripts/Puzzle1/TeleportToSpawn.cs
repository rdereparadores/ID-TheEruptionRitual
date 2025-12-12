using System;
using UnityEngine;

public class TeleportToSpawn : MonoBehaviour
{
    public Transform spawnTransform;

    private Rigidbody _rigidbody;
    private bool _firstTime = true;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            transform.position = spawnTransform.position;
            
            if (_firstTime)
            {
                PlayerDialoguesHandler.Singleton.playPuzzle2OnKeyDropRpc();
                _firstTime = false;
            }
        }
    }
}
