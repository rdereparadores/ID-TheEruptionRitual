using UnityEngine;

public class DoorLockDialogueTrigger : MonoBehaviour
{
    private bool _firstTime = true;
    
    public void Play()
    {
        if (_firstTime)
        {
            PlayerDialoguesHandler.Singleton.playPuzzle3OnDoorUnlock();
            _firstTime = false;
        }
    }
}
