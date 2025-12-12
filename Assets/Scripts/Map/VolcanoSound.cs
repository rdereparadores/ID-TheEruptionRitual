using UnityEngine;
using System.Collections; 

public class EmisorPeriodico : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("El intervalo de tiempo en segundos")]
    [SerializeField] private float intervalo = 120f; 

    [Header("Componentes")]
    [SerializeField] private AudioSource fuenteDeAudio;

    private void Start()
    {
        if (fuenteDeAudio == null)
        {
            fuenteDeAudio = GetComponent<AudioSource>();
        }

        StartCoroutine(RutinaDeSonido());
    }

    private IEnumerator RutinaDeSonido()
    {
        while (true)
        {
            if (fuenteDeAudio != null && fuenteDeAudio.clip != null)
            {
                fuenteDeAudio.Play();
            }
            
            yield return new WaitForSeconds(intervalo);
        }
    }
}