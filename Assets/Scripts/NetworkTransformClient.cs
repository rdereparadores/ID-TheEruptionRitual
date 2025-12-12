using Unity.Netcode.Components;
using UnityEngine;

public class NetworkTransformClient : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
