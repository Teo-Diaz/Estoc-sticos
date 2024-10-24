using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerScreen : MonoBehaviour
{
    public GameObject Screen;
    public GameObject Controlador;

    // Función que detecta la colisión con el trigger en 2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra en el trigger tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Pausar el juego
            Time.timeScale = 0f;

            // Mostrar la pantalla de ganador
            Screen.SetActive(true);
            Controlador.SetActive(false);
        }
    }
}

