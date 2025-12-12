using UnityEngine;

public class SpawnearPelota : MonoBehaviour
{
    public GameObject pelotaPrefab; // Asigna el prefab de la pelota blanca en el Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Método para spawnear la pelota al presionar el botón
    public void spawn()
    {
        if (pelotaPrefab != null)
        {
            // Calcula la posición: justo encima (Y + 1) y ligeramente adelante (Z + 0.5)
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0.5f);
            Instantiate(pelotaPrefab, spawnPosition, Quaternion.identity);
        }
    }
}