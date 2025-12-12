using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NetworkGrabbable : NetworkBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private NetworkObject _netObject;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _netObject = GetComponent<NetworkObject>();
    }

    public override void OnNetworkSpawn()
    {
        _grabInteractable.selectEntered.AddListener(OnGrab);
    }

    public override void OnNetworkDespawn()
    {
        _grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (IsOwner) return;
        
        RequestObjectOwnershipRpc(NetworkManager.Singleton.LocalClientId);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void RequestObjectOwnershipRpc(ulong newOwnerId)
    {
        _netObject.ChangeOwnership(newOwnerId);
    }
}
