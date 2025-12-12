using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class PlayerGameStartHandler : NetworkBehaviour
{
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    public TeleportationArea player1TeleportationArea;
    public TeleportationArea player2TeleportationArea;

    private Transform _currentPlayerTransform;

    public void Start()
    {
        _currentPlayerTransform = GetComponent<Transform>();
    }

    public void TeleportToSpawnPoint()
    {
        _currentPlayerTransform.position = NetworkManager.Singleton.IsHost ? player1SpawnPoint.position : player2SpawnPoint.position;
        if (IsHost)
        {
            player2TeleportationArea.enabled = false;
        }
        else
        {
            player1TeleportationArea.enabled = false;
        }
        PlayerDialoguesHandler.Singleton.playPregame();
    }

    [ClientRpc]
    public void TeleportClientToSpawnPointClientRpc()
    {
        TeleportToSpawnPoint();
    }

}
