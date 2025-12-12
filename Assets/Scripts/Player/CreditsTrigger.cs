using Unity.Netcode;
using UnityEngine;

public class CreditsTrigger : NetworkBehaviour
{
    public MeshCollider ccollider;
    
    private void OnTriggerEnter(Collider other)
    {
        CreditsRpc();
    }
    
    [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
    private void CreditsRpc()
    {
        ccollider.enabled = false;
    }
}
