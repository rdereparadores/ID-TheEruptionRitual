using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class VerificadorReceta : NetworkBehaviour
{
    [Header("La Receta Ganadora")]
    [Tooltip("Añade aquí los colores exactos que debe tener la pelota (ej: Rojo y Azul)")]
    public List<Color> ingredientesNecesarios; 

    [Header("Configuración")]
    public float toleranciaColor = 0.05f;
    public AudioClip sonidoVictoria;
    public GameObject laser;

    private AudioSource _audioSource;
    private bool _resuleto = false;

    void Start()
    {
        Debug.Log("Iniciando Verificador de Receta...");
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("¡Algo ha tocado el trigger! Objeto: " + other.name);

        // 1. Buscamos el script de la pelota
        ColorPelota scriptPelota = other.GetComponent<ColorPelota>();

        if (scriptPelota != null && !_resuleto)
        {
            // 2. Extraemos los colores que la pelota tiene activos actualmente
            List<Color> ingredientesPelota = ObtenerIngredientesDeLaPelota(scriptPelota);

            // 3. Comparamos las dos listas (Receta vs Pelota)
            if (ComprobarSiLaRecetaEsCorrecta(ingredientesPelota))
            {
                Ganar();
            }
            else
            {
                Debug.Log("Receta incorrecta. Sigue intentando.");
            }
        }
    }

    // Extrae la lista de colores de la pelota
    List<Color> ObtenerIngredientesDeLaPelota(ColorPelota pelota)
    {
        // Simplemente devolvemos una copia de la lista de colores de la pelota.
        return new List<Color>(pelota.coloresActuales);
    }

    // Lógica principal de comparación
    bool ComprobarSiLaRecetaEsCorrecta(List<Color> ingredientesPelota)
    {
    // Si la pelota está vacía, tampoco ganamos.
        if (ingredientesPelota.Count == 0) return false;

        // A. Si no tienen la misma cantidad de ingredientes, ya falló.
        if (ingredientesPelota.Count != ingredientesNecesarios.Count)
        {
            return false;
        }

        // B. Creamos una copia temporal de la pelota para ir tachando ingredientes
        List<Color> copiaPelota = new List<Color>(ingredientesPelota);

        // C. Recorremos nuestra receta ganadora
        foreach (Color colorRequerido in ingredientesNecesarios)
        {
            bool encontrado = false;

            // Buscamos si este color requerido está en la pelota
            for (int i = 0; i < copiaPelota.Count; i++)
            {
                if (EsMismoColor(colorRequerido, copiaPelota[i]))
                {
                    // ¡Encontrado! Lo tachamos (removemos) para no contarlo dos veces
                    copiaPelota.RemoveAt(i);
                    encontrado = true;
                    break; // Salimos del bucle interno y vamos al siguiente ingrediente
                }
            }

            // Si terminamos de buscar en la pelota y no encontramos este ingrediente... falló.
            if (!encontrado) return false;
        }

        // Si pasamos todos los checks, es la combinación correcta
        return true;
    }

    bool EsMismoColor(Color c1, Color c2)
    {
        float dif = Mathf.Abs(c1.r - c2.r) + Mathf.Abs(c1.g - c2.g) + Mathf.Abs(c1.b - c2.b);
        return dif < toleranciaColor;
    }

    void Ganar()
    {
        _resuleto = true;
        Debug.Log("¡VICTORIA! Receta completada.");
        if (sonidoVictoria != null) _audioSource.PlayOneShot(sonidoVictoria);
        laser.GetComponent<Laser>().enabled = true;
        EnableLaserClientRpc();
        
        PlayerDialoguesHandler.Singleton.playPuzzle3StartRpc();
    }

    [ClientRpc]
    private void EnableLaserClientRpc()
    {
        laser.GetComponent<Laser>().enabled = true;
    }
}