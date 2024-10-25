using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esto para trabajar con UI

public class DeathTrigger : MonoBehaviour
{
    public List<Transform> SpawnPlayers; // Lista de puntos de spawn para los jugadores
    public Button actionButton; // Referencia al botón de la UI
    [Range(0, 100)]
    public int deathProbability = 50; // Probabilidad de muerte (0% a 100%)
    public int multiplicador = 2; // Multiplicador de ganancias

    private System.Random random = new System.Random();

    // Lista que almacena las probabilidades de muerte de cada casilla
    public List<int> deathProbabilities; // Asegúrate de inicializar esta lista
    private List<int> originalProbabilities; // Almacena las probabilidades originales

    // Referencia a la UI de puntajes
    public PlayerScoreUI playerScoreUI;

   

    // Referencia al Canvas que quieres activar/desactivar
    public GameObject probabilityCanvas; // Asegúrate de asignar este Canvas en el Inspector

    private void Start()
    {
        // Inicializar las probabilidades de muerte (ejemplo)
        deathProbabilities = new List<int> { 80, 50, 20, 0 }; // Ejemplo de probabilidades para 4 casillas
        originalProbabilities = new List<int>(deathProbabilities); // Almacenar las probabilidades originales

        // Asignar el listener para el botón
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnButtonClick);
        }

        // Actualizar el texto al iniciar
        UpdateProbabilityText();
        probabilityCanvas.SetActive(false); // Asegurarse de que el canvas esté desactivado al inicio
    }

    private void OnButtonClick()
    {
        // Verificar si hay algún jugador en una casilla con probabilidad 0
        bool hasZeroProbability = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f); // Ajusta el radio según sea necesario

        foreach (var collider in colliders)
        {
            int playerLayer = collider.gameObject.layer;

            if (playerLayer == LayerMask.NameToLayer("P1") ||
                playerLayer == LayerMask.NameToLayer("P2") ||
                playerLayer == LayerMask.NameToLayer("P3") ||
                playerLayer == LayerMask.NameToLayer("P4"))
            {
                int playerIndex = GetPlayerIndex(LayerMask.LayerToName(playerLayer));

                if (deathProbabilities[playerIndex] == 0)
                {
                    hasZeroProbability = true;
                    break; // Salir del bucle si encontramos un 0
                }
            }
        }

        if (!hasZeroProbability)
        {
            // Restaurar las probabilidades originales si no hay probabilidad 0
            deathProbabilities = new List<int>(originalProbabilities); // Restablecer a las probabilidades originales
            UpdateProbabilityText("Las probabilidades han sido restauradas a su estado original.");
            Debug.Log("Las probabilidades han sido restauradas a su estado original.");
            probabilityCanvas.SetActive(false); // Desactiva el canvas
        }
        else
        {
            // Aumentar todas las probabilidades en 15%
            for (int i = 0; i < deathProbabilities.Count; i++)
            {
                deathProbabilities[i] = Mathf.Min(deathProbabilities[i] + 15, 100); // Aumentar y asegurar que no supere 100
            }

            // Muestra las nuevas probabilidades de muerte de cada casilla
            Debug.Log("Nuevas probabilidades de muerte después del aumento:");
            for (int i = 0; i < deathProbabilities.Count; i++)
            {
                Debug.Log("Probabilidad de la casilla " + (i + 1) + ": " + deathProbabilities[i] + "%");
            }
            UpdateProbabilityText("Las probabilidades han aumentado. Nuevas probabilidades:");
            probabilityCanvas.SetActive(true); // Activa el canvas
        }

        // Continuar con la lógica de muerte y supervivencia de jugadores
        foreach (var collider in colliders)
        {
            int playerLayer = collider.gameObject.layer;

            if (playerLayer == LayerMask.NameToLayer("P1") ||
                playerLayer == LayerMask.NameToLayer("P2") ||
                playerLayer == LayerMask.NameToLayer("P3") ||
                playerLayer == LayerMask.NameToLayer("P4"))
            {
                string playerTag = LayerMask.LayerToName(playerLayer);
                int playerIndex = GetPlayerIndex(playerTag); // Obtener el índice del jugador

                int chance = random.Next(0, 100); // Genera un número aleatorio entre 0 y 99

                // Compara la probabilidad de muerte con el valor aleatorio generado
                if (chance < deathProbabilities[playerIndex])
                {
                    // El jugador muere, reposicionar y restar tarifa
                    if (playerIndex >= 0 && playerIndex < SpawnPlayers.Count)
                    {
                        // Reposicionar al jugador en su Spawn correspondiente
                        collider.transform.position = SpawnPlayers[playerIndex].position;

                        // Aumentar la tarifa en 100
                        playerScoreUI.tarifa += 100; // Aumenta la tarifa en 100
                        playerScoreUI.UpdateTarifaUI(); // Actualiza la UI de la tarifa
                        playerScoreUI.UpdateScore(playerTag, -playerScoreUI.tarifa); // Resta 100 puntos por la tarifa
                        Debug.Log(playerTag + " ha sido reposicionado a su Spawn y se le ha restado la tarifa.");
                    }
                }
                else
                {
                    if (playerScoreUI.GetPlayerScore(playerTag) > 0)
                    {
                        int scoreChange = Mathf.FloorToInt(playerScoreUI.GetPlayerScore(playerTag) * (multiplicador - 1)); // Calcula el cambio de puntaje
                        playerScoreUI.UpdateScore(playerTag, scoreChange); // Actualiza el puntaje
                        Debug.Log(playerTag + " ha sobrevivido y sus puntos han sido multiplicados.");
                    }
                    else
                    {
                        // Desactivar el BoxCollider2D si el puntaje es cero o menor
                        BoxCollider2D boxCollider = collider.GetComponent<BoxCollider2D>();
                        if (boxCollider != null)
                        {
                            boxCollider.enabled = false; // Desactiva el collider
                            Debug.Log(playerTag + " está descalificado y su BoxCollider ha sido desactivado.");
                        }
                    }
                }
            }
        }

        // Verificar si ya no hay jugadores en una casilla de probabilidad 0
        CheckForZeroProbabilityPlayers();
    }

    // Método para obtener el índice de spawn del jugador basado en su capa
    private int GetPlayerIndex(string playerTag)
    {
        switch (playerTag)
        {
            case "P1":
                return 0; // Índice para P1
            case "P2":
                return 1; // Índice para P2
            case "P3":
                return 2; // Índice para P3
            case "P4":
                return 3; // Índice para P4
            default:
                return -1; // Retorna -1 si no se encuentra el jugador
        }
    }

    // Método para actualizar el texto de probabilidad en la UI
    private void UpdateProbabilityText(string message = "")
    {
        if (string.IsNullOrEmpty(message))
        {
            message = "Probabilidades actuales: " + string.Join(", ", deathProbabilities);
        }
        //debug.log(mesage);
    }

    // Método para verificar si hay jugadores en casillas de probabilidad 0
    private void CheckForZeroProbabilityPlayers()
    {
        bool hasZeroProbability = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f); // Ajusta el radio según sea necesario

        foreach (var collider in colliders)
        {
            int playerLayer = collider.gameObject.layer;

            if (playerLayer == LayerMask.NameToLayer("P1") ||
                playerLayer == LayerMask.NameToLayer("P2") ||
                playerLayer == LayerMask.NameToLayer("P3") ||
                playerLayer == LayerMask.NameToLayer("P4"))
            {
                int playerIndex = GetPlayerIndex(LayerMask.LayerToName(playerLayer));

                if (deathProbabilities[playerIndex] == 0)
                {
                    hasZeroProbability = true;
                    break; // Salir del bucle si encontramos un 0
                }
            }
        }

        // Desactiva el canvas si no hay jugadores en casillas de probabilidad 0
        if (!hasZeroProbability)
        {
            probabilityCanvas.SetActive(false);
        }
    }
}
