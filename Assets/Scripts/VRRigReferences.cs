using System;
using UnityEngine;

public class VRRigReferences : MonoBehaviour
{
    public static VRRigReferences Singleton;
    
    public Transform root;
    public Transform leftHand;
    public Transform rightHand;
    public Transform head;

    private void Awake()
    {
        Singleton = this;
    }
}
