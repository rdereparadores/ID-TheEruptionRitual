using UnityEngine;
using System.Collections.Generic; // Necesario para usar listas
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]

public class ColorPelota : NetworkBehaviour
{
    [Header("Configuración")]
    [Tooltip("El número máximo de colores que se pueden mezclar en la pelota.")]
    public int maxColores = 3;

    [Header("Estado Actual (Solo lectura)")]
    [Tooltip("Los colores que contiene la pelota actualmente.")]
    public List<Color> coloresActuales = new List<Color>();

    [Header("Audio")]
    [Tooltip("Sonido que se reproduce al pintar la pelota.")]
    public AudioClip sonidoPintura;
    private Renderer rend;
    private AudioSource audioSource;

    void Start()
    {
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        UpdateBallColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        ProveedorColor provider = other.GetComponent<ProveedorColor>();
        if (provider != null)
        {
            AddColor(provider.cubeColor);
        }
    }

    void AddColor(Color newColor)
    {
        // Añadimos el nuevo color a la lista
        coloresActuales.Add(newColor);

        // Si hemos superado el máximo de colores, eliminamos el más antiguo (el primero de la lista)
        while (coloresActuales.Count > maxColores)
        {
            coloresActuales.RemoveAt(0);
        }
        
        // Calculamos el color final y actualizamos el material
        UpdateBallColor();
    }

    void UpdateBallColor()
    {
        if (coloresActuales.Count == 0)
        {
            rend.material.color = Color.white; // O el color base que quieras
            return;
        }

        // --- LÓGICA DE MEZCLA ---
        
        float r = 0, g = 0, b = 0;

        foreach (Color c in coloresActuales)
        {
            r += c.r;
            g += c.g;
            b += c.b;
        }

        // Promedio matemático: Suma de colores / Cantidad de colores
        Color finalColor = new Color(r / coloresActuales.Count, g / coloresActuales.Count, b / coloresActuales.Count, 1f);

        // --- CORRECCIÓN IMPORTANTE PARA ROJO-AMARILLO-AZUL ---    
        finalColor = CorregirMezclaPintura(finalColor); 

        rend.material.color = finalColor;

        // Reproducimos el sonido de pintura
        if (sonidoPintura != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoPintura);
        }
    }

    // Función opcional para simular pintura real
    Color CorregirMezclaPintura(Color colorMezclado)
    {
        // Detectamos si la mezcla ha dado ese "gris feo" típico de Azul(0,0,1) + Amarillo(1,1,0)
        // El gris matemático suele ser (0.5, 0.5, 0.5) aprox.
        
        bool tieneAzul = false;
        bool tieneAmarillo = false;

        foreach(Color c in coloresActuales)
        {
            if (c.b > 0.8f && c.r < 0.2f) tieneAzul = true;      // Es azul
            if (c.r > 0.8f && c.g > 0.8f) tieneAmarillo = true;  // Es amarillo
        }

        // Si tenemos azul y amarillo en la mezcla, forzamos un verde
        if (tieneAzul && tieneAmarillo)
        {
             // Si también hay rojo, se vuelve marrón, si no, es verde
             float rojoPromedio = colorMezclado.r;
             if (rojoPromedio > 0.3f) return new Color(0.4f, 0.2f, 0.1f); // Marrón sucio (las 3 mezclas)
             return Color.green; // Verde bonito
        }

        return colorMezclado;
    }
}