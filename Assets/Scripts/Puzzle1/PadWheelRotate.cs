using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PadWheelRotate : MonoBehaviour
{
    public enum WheelPosition
    {
        SQUARE = 0,
        STAR = 1,
        PENTAGON = 2,
        CIRCLE = 3,
        TRIANGLE = 4
    }
    
    // 72 grados -> 5 posiciones
    public float snapAngle = 72f;
    
    // Grados por metro de movimiento
    public float sensitivity = 360f;

    public WheelPosition position = WheelPosition.SQUARE;
    
    private XRBaseInteractor _interactor;
    private Vector3 _lastHandPosition;

    public void OnGrabStart(SelectEnterEventArgs args)
    {
        _interactor = args.interactorObject as XRBaseInteractor;
        _lastHandPosition = _interactor.transform.position;
    }

    public void OnGrabEnd(SelectExitEventArgs args)
    {
        _interactor = null;
        float angle = transform.localEulerAngles.x;
        
        // Si ha cambiado el eje Y o el Z, Unity ha invertido el eje X para evitar el Gimbal Lock
        // Sólo pasa con el eje X (putada)
        if (Mathf.Abs(transform.localEulerAngles.y) > 0.1f || Mathf.Abs(transform.localEulerAngles.z) > 0.1f)
        {
            angle = 180 - angle;
        }
        
        // Si el ángulo es negativo lo invierte, y si es 360 lo pasa a 1
        // (por perder 1 grado de resolución no se acaba el mundo)
        angle = Mathf.Repeat(angle, 359f);
        int pos = Mathf.RoundToInt(angle / snapAngle);
        float adjustedAngle = snapAngle * pos;
        transform.localEulerAngles = new Vector3(adjustedAngle, 0, 0);

        position = (WheelPosition)pos;
    }

    private void Update()
    {
        if (!_interactor) return;
        
        Vector3 handPosition = _interactor.transform.position;
        float delta = handPosition.y - _lastHandPosition.y;
        transform.Rotate(delta * sensitivity, 0f, 0f);
        _lastHandPosition = handPosition;
    }
}
