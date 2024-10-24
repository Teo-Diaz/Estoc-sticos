using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esto para trabajar con UI

public class DeathTrigger : MonoBehaviour
{
    public List<Transform> SpawnPlayers; // Lista de puntos de spawn para los jugadores
    public Button actionButton; // Referencia al botón de la UI
    [Range(0, 100)] // Esto permitirá que se ajuste en el Inspector de Unity
    public int deathProbability = 50; // Probabilidad de muerte (0% a 100%)
    public int multiplicador = 2; // Multiplicador de ganancias

    private System.Random random = new System.Random();

    // Diccionario para almacenar el puntaje inicial de cada jugador
    private Dictionary<string, int> playerScores = new Dictionary<string, int>
    {
        { "P1", 1000 },
        { "P2", 1000 },
        { "P3", 1000 },
        { "P4", 1000 }
    };

    private void Start()
    {
        // Asegúrate de que el botón tenga la referencia adecuada y suscriba el evento
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnButtonClick);
        }
    }

    // Método que se llama cuando se presiona el botón
    private void OnButtonClick()
    {
        // Verifica todos los colliders en el trigger
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f); // Ajusta el radio según sea necesario

        foreach (var collider in colliders)
        {
            // Verificamos si el objeto que toca el trigger pertenece a las capas de los jugadores
            int playerLayer = collider.gameObject.layer;

            if (playerLayer == LayerMask.NameToLayer("P1") ||
                playerLayer == LayerMask.NameToLayer("P2") ||
                playerLayer == LayerMask.NameToLayer("P3") ||
                playerLayer == LayerMask.NameToLayer("P4"))
            {
                int chance = random.Next(0, 100); // Genera un número aleatorio entre 0 y 99

                // Compara la probabilidad de muerte con el valor aleatorio generado
                if (chance < deathProbability)
                {
                    // El jugador muere, reposicionar
                    string playerTag = LayerMask.LayerToName(playerLayer);
                    int playerIndex = GetPlayerIndex(playerTag); // Obtiene el índice de spawn correspondiente

                    if (playerIndex >= 0 && playerIndex < SpawnPlayers.Count)
                    {
                        // Reposicionar al jugador en su Spawn correspondiente
                        collider.transform.position = SpawnPlayers[playerIndex].position;

                        // Quitar 100 puntos al jugador
                        if (playerScores.ContainsKey(playerTag))
                        {
                            playerScores[playerTag] -= 100;
                            Debug.Log(playerTag + " ha sido reposicionado a su Spawn y se le han quitado 100 puntos. Puntos restantes: " + playerScores[playerTag]);
                        }
                    }
                }
                else
                {
                    // Si no muere, multiplica sus puntos por el multiplicador
                    string playerTag = LayerMask.LayerToName(playerLayer);
                    if (playerScores.ContainsKey(playerTag))
                    {
                        playerScores[playerTag] *= multiplicador; // Multiplica los puntos por el multiplicador
                        Debug.Log(playerTag + " ha sobrevivido y sus puntos han sido multiplicados. Puntos totales: " + playerScores[playerTag]);
                    }
                }
            }
        }
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
}
