using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SonidoRebote : MonoBehaviour
{
    public AudioSource audioSource; // Referencia pública al AudioSource
    public float volumenBase = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 0.1f)
        {
            // El volumen depende de la fuerza del impacto (clampeado a máximo 1)
            float volumen = Mathf.Clamp01(collision.relativeVelocity.magnitude / 10f);
            audioSource.volume = volumen * volumenBase;

            audioSource.pitch = Random.Range(0.8f, 1.2f); // Varía el tono aleatoriamente
            audioSource.Play();
        }
    }
}
