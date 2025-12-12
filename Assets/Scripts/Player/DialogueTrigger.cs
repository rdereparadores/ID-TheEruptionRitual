using System;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public UnityEvent dialogueCallback;
    private bool _alreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_alreadyTriggered)
        {
            dialogueCallback.Invoke();
            _alreadyTriggered = true;
        }
    }
}
