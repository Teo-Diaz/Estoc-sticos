using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathTrigger : MonoBehaviour
{
    public List<Transform> SpawnPlayers; // Lista de puntos de spawn para los jugadores
    public Button actionButton; // Referencia al bot�n de la UI
    [Range(0, 100)] // Esto permitir� que se ajuste en el Inspector de Unity
    public int deathProbability = 50; // Probabilidad de muerte (0% a 100%)
    public float multiplicador = 1.5f; // Multiplicador de ganancias


    private System.Random random = new System.Random();

    // Referencia a PlayerScoreUI para actualizar puntajes
    public PlayerScoreUI playerScoreUI;

    private void Start()
    {
        // Aseg�rate de que el bot�n tenga la referencia adecuada y suscriba el evento
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnButtonClick);
        }
    }

    // M�todo que se llama cuando se presiona el bot�n
    private void OnButtonClick()
    {
        // Verifica todos los colliders en el trigger
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f); // Ajusta el radio seg�n sea necesario

        foreach (var collider in colliders)
        {
            // Verificamos si el objeto que toca el trigger pertenece a las capas de los jugadores
            int playerLayer = collider.gameObject.layer;

            if (playerLayer == LayerMask.NameToLayer("P1") ||
                playerLayer == LayerMask.NameToLayer("P2") ||
                playerLayer == LayerMask.NameToLayer("P3") ||
                playerLayer == LayerMask.NameToLayer("P4"))
            {
                int chance = random.Next(0, 100); // Genera un n�mero aleatorio entre 0 y 99

                // Compara la probabilidad de muerte con el valor aleatorio generado
                string playerTag = LayerMask.LayerToName(playerLayer);
                if (chance < deathProbability)
                {
                    // El jugador muere, reposicionar y restar tarifa
                    int playerIndex = GetPlayerIndex(playerTag); // Obtiene el �ndice de spawn correspondiente

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
                    // Si no muere y tiene m�s de 0 puntos, multiplica sus puntos por el multiplicador
                    if (playerScoreUI.GetPlayerScore(playerTag) > 0)
                    {
                        int scoreChange = Mathf.FloorToInt(playerScoreUI.GetPlayerScore(playerTag) * (multiplicador - 1)); // Calcula el cambio de puntaje
                        playerScoreUI.UpdateScore(playerTag, scoreChange); // Actualiza el puntaje
                        Debug.Log(playerTag + " ha sobrevivido y sus puntos han sido multiplicados.");
                    }
                    else
                    {
                        Debug.Log(playerTag + " est� descalificado y no recibe bonificaciones.");
                    }
                }
            }
        }
    }


    // M�todo para obtener el �ndice de spawn del jugador basado en su capa
    private int GetPlayerIndex(string playerTag)
    {
        switch (playerTag)
        {
            case "P1":
                return 0; // �ndice para P1
            case "P2":
                return 1; // �ndice para P2
            case "P3":
                return 2; // �ndice para P3
            case "P4":
                return 3; // �ndice para P4
            default:
                return -1; // Retorna -1 si no se encuentra el jugador
        }
    }
}
