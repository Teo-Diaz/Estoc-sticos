using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esto para trabajar con UI

public class WinnerScreen : MonoBehaviour
{
    public GameObject Screen;
    public GameObject Controlador;
    public TextMeshProUGUI winnerText; // Añade una referencia al componente Text

    // Función que detecta la colisión con el trigger en 2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra en el trigger tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Pausar el juego
            Time.timeScale = 0f;

            // Mostrar la pantalla de ganador
            Screen.SetActive(true);
            Controlador.SetActive(false);

            // Obtener la capa del jugador
            string layerName = LayerMask.LayerToName(other.gameObject.layer);

            // Actualizar el texto para mostrar la capa del jugador
            winnerText.text = "¡Ganador: " + layerName + "!";
        }
    }
}
