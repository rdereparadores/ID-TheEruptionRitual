using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkAnimateHandOnInput : NetworkBehaviour
{
    public InputActionProperty triggerValue;
    public InputActionProperty gripValue;

    public Animator handAnimator;

    private void Update()
    {
        if (IsOwner)
        {
            float trigger = triggerValue.action.ReadValue<float>();
            float grip = gripValue.action.ReadValue<float>();
        
            handAnimator.SetFloat("Trigger", trigger);
            handAnimator.SetFloat("Grip", grip);
        }
    }
}
