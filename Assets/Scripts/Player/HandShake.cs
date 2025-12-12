using UnityEngine;
using UnityEngine.XR; // Necesario para detectar los mandos
using System.Collections;
using System.Collections.Generic;

public class TemblorVRHaptico : MonoBehaviour
{
    [Header("Configuración Visual")]
    [Tooltip("El objeto PADRE de la cámara. Déjalo vacío si solo quieres vibración.")]
    [SerializeField] private Transform contenedorDeCamara;
    [SerializeField] private bool activarMovimientoVisual = true;
    [SerializeField] private float magnitudVisual = 0.01f; // Muy sutil

    [Header("Configuración Háptica (Vibración)")]
    [Tooltip("Fuerza de la vibración (0 a 1)")]
    [Range(0, 1)] [SerializeField] private float fuerzaVibracion = 0.5f;
    
    [Header("Tiempos")]
    [SerializeField] private float intervaloMinutos = 2f; // Cada 2 minutos
    [SerializeField] private float duracionTemblor = 2.0f;

    private void Start()
    {
        StartCoroutine(CicloDeTemblor());
    }

    private IEnumerator CicloDeTemblor()
    {
        while (true)
        {
            // Convertimos minutos a segundos
            yield return new WaitForSeconds(intervaloMinutos * 60f);

            // Iniciamos el efecto
            yield return StartCoroutine(EjecutarEfecto());
        }
    }

    private IEnumerator EjecutarEfecto()
    {
        // 1. Disparamos la vibración en ambos mandos
        // La vibración se manda una vez con la duración total
        VibrarMando(XRNode.LeftHand, fuerzaVibracion, duracionTemblor);
        VibrarMando(XRNode.RightHand, fuerzaVibracion, duracionTemblor);

        // 2. Si está activado, movemos la cámara visualmente
        if (activarMovimientoVisual && contenedorDeCamara != null)
        {
            Vector3 posicionOriginal = contenedorDeCamara.localPosition;
            float tiempo = 0.0f;

            while (tiempo < duracionTemblor)
            {
                float x = Random.Range(-1f, 1f) * magnitudVisual;
                float y = Random.Range(-1f, 1f) * magnitudVisual;
                
                contenedorDeCamara.localPosition = new Vector3(posicionOriginal.x + x, posicionOriginal.y + y, posicionOriginal.z);
                
                tiempo += Time.deltaTime;
                yield return null;
            }
            // Restaurar posición
            contenedorDeCamara.localPosition = posicionOriginal;
        }
        else
        {
            // Si no hay movimiento visual, solo esperamos a que termine la duración
            yield return new WaitForSeconds(duracionTemblor);
        }
    }

    // Función auxiliar para buscar el mando y enviarle la señal
    private void VibrarMando(XRNode mano, float fuerza, float duracion)
    {
        InputDevice dispositivo = InputDevices.GetDeviceAtXRNode(mano);

        if (dispositivo.isValid)
        {
            // channel 0 es el motor principal de vibración
            dispositivo.SendHapticImpulse(0, fuerza, duracion);
        }
    }
}