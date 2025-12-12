using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public enum Phase { Pregame, Puzzle1, Puzzle2, Puzzle3 }

public class PlayerDialoguesHandler : NetworkBehaviour
{
    public static PlayerDialoguesHandler Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    public AudioSource narratorAudioSource;
    public AudioSource player1AudioSource;
    public AudioSource player2AudioSource;
    
    // PRE-GAME
    public AudioClip pregameNarrator;
    public AudioClip pregamePlayer2;
    
    // PUZZLE 1
    public AudioClip puzzle1Player1OnLockApproach;
    public AudioClip puzzle1Player2OnHintApproach;
    public AudioClip puzzle1Player1OnCorrectCode;
    
    // PUZZLE 2
    public AudioClip puzzle2Player2Start;
    public AudioClip puzzle2NarratorOnKeyDrop;
    public AudioClip puzzle2Player2OnChestUnlock;
    public AudioClip puzzle2Player1OnChestUnlock;
    
    // PUZZLE 3
    public AudioClip puzzle3Player2Start;
    public AudioClip puzzle3NarratorOnMuchTimeLeft;
    public AudioClip puzzle3Player1Start;
    public AudioClip puzzle3Player2OnDoorUnlock;
    public AudioClip puzzle3Player1OnPlayer2DoorUnlock;
    public AudioClip puzzle3Player2OnDoorUnlockResponse;

    public Phase phase = Phase.Pregame;

    public async void playPregame()
    {
        await Task.Delay(2000);
        narratorAudioSource.PlayOneShot(pregameNarrator);
        await Task.Delay((int)pregameNarrator.length * 1000 + 1500);
        player2AudioSource.PlayOneShot(pregamePlayer2);
        CountdownHandler.Singleton.StartCountdown();
        phase = Phase.Puzzle1;
    }
    
    [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
    public void playPuzzle1LockApproachRpc()
    {
        if (phase == Phase.Puzzle1)
        {
            player1AudioSource.PlayOneShot(puzzle1Player1OnLockApproach);
        }
    }
    
    [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
    public void playPuzzle1HintApproachRpc()
    {
        if (phase == Phase.Puzzle1)
        {
            player2AudioSource.PlayOneShot(puzzle1Player2OnHintApproach);
        }
    }
    
    [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
    public void playPuzzle2StartRpc()
    {
        playPuzzle2Start();
    }
    
    public async void playPuzzle2Start()
    {
        player1AudioSource.PlayOneShot(puzzle1Player1OnCorrectCode);
        await Task.Delay((int)puzzle1Player1OnCorrectCode.length * 1000 + 800);
        player2AudioSource.PlayOneShot(puzzle2Player2Start);
        phase = Phase.Puzzle2;
    }
    
    [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
    public void playPuzzle2OnKeyDropRpc()
    {
        if (phase == Phase.Puzzle2)
        {
            narratorAudioSource.PlayOneShot(puzzle2NarratorOnKeyDrop);
        }
    }
    
    public async void playPuzzle2OnChestUnlock()
    {
        if (phase == Phase.Puzzle2)
        {
            player2AudioSource.PlayOneShot(puzzle2Player2OnChestUnlock);
            await Task.Delay((int)puzzle2Player2OnChestUnlock.length * 1000 + 1000);
            player1AudioSource.PlayOneShot(puzzle2Player1OnChestUnlock);
        }
    }

    [Rpc(SendTo.Everyone, InvokePermission = RpcInvokePermission.Everyone)]
    public void playPuzzle3StartRpc()
    {
        playPuzzle3Start();
    }
    
    public async void playPuzzle3Start()
    {
        player2AudioSource.PlayOneShot(puzzle3Player2Start);
        await Task.Delay((int)puzzle3Player2Start.length * 1000 + 1500);
        
        // SOLO SI QUEDAN MENOS DE 3 MIN EN EL CONTADOR
        narratorAudioSource.PlayOneShot(puzzle3NarratorOnMuchTimeLeft);
        await Task.Delay((int)puzzle3NarratorOnMuchTimeLeft.length * 1000 + 1000);
        
        player1AudioSource.PlayOneShot(puzzle3Player1Start);
        
        phase = Phase.Puzzle3;
        CountdownHandler.Singleton.remainingTime = 180f;
    }
    
    public async void playPuzzle3OnDoorUnlock()
    {
        player2AudioSource.PlayOneShot(puzzle3Player2OnDoorUnlock);
        await Task.Delay((int)puzzle3Player2OnDoorUnlock.length * 1000 + 1000);
        player1AudioSource.PlayOneShot(puzzle3Player1OnPlayer2DoorUnlock);
        await Task.Delay((int)puzzle3Player2OnDoorUnlockResponse.length * 1000 + 1000);
    }
}
