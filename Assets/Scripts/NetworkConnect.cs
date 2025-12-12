using System;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;

public class NetworkConnect : MonoBehaviour
{
    public int maxConnections = 2;
    public UnityTransport transport;
    public TMP_Text joinCodePlaceholder;
    public GameObject waitingForPlayerText;
    public TMP_InputField joinCodeInput;
    public GameObject startGameButton;
    
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectCallback;
    }

    public async void Create()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        string newJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        joinCodePlaceholder.text = "Código: " + newJoinCode;
        waitingForPlayerText.SetActive(true);
        
        transport.SetHostRelayData(
            allocation.RelayServer.IpV4,
            (ushort) allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
        );
        
        NetworkManager.Singleton.StartHost();
    }

    public async void Join()
    {
        string joinCode = joinCodeInput.text;
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        
        transport.SetClientRelayData(
            allocation.RelayServer.IpV4,
            (ushort) allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData,
            allocation.HostConnectionData
        );
        
        NetworkManager.Singleton.StartClient();
    }

    private void OnClientConnectCallback(ulong clientId)
    {
        if (clientId != NetworkManager.ServerClientId && NetworkManager.Singleton.IsHost)
        {
            startGameButton.SetActive(true);
            waitingForPlayerText.SetActive(false);
        }
    }
}
