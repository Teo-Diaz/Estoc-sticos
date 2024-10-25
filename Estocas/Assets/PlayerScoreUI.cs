using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI scoreTextP2;
    public TextMeshProUGUI scoreTextP3;
    public TextMeshProUGUI scoreTextP4;
    public TextMeshProUGUI tarifaText;

    private Dictionary<string, int> playerScores = new Dictionary<string, int>
    {
        { "P1", 1000 },
        { "P2", 1000 },
        { "P3", 1000 },
        { "P4", 1000 }
    };

    public int tarifa = 100; // Tarifa inicial

    private void Start()
    {
        UpdateScoreUI(); // Actualizar la UI al inicio
        UpdateTarifaUI(); // Mostrar tarifa inicial
    }

    // Método para actualizar el puntaje de un jugador
    public void UpdateScore(string playerTag, int scoreChange)
    {
        if (playerScores.ContainsKey(playerTag))
        {
            // Solo actualiza el puntaje si es mayor a cero
            if (playerScores[playerTag] > 0)
            {
                playerScores[playerTag] += scoreChange;

                // Verifica si el puntaje es menor o igual a cero
                if (playerScores[playerTag] <= 0)
                {
                    playerScores[playerTag] = 0; // Asegúrate de que no sea negativo
                    Debug.Log(playerTag + " ha sido descalificado.");
                    // Aquí puedes descalificar al jugador o llamar a un método para hacerlo
                }

                UpdateScoreUI(); // Actualiza la UI del puntaje
            }
        }
    }

    // Método para obtener el puntaje de un jugador
    public int GetPlayerScore(string playerTag)
    {
        if (playerScores.ContainsKey(playerTag))
        {
            return playerScores[playerTag];
        }
        return 0; // Devuelve 0 si el jugador no está en el diccionario
    }

    // Método para actualizar la UI de los puntajes
    public void UpdateScoreUI()
    {
        scoreTextP1.text = "P1: " + playerScores["P1"].ToString();
        scoreTextP2.text = "P2: " + playerScores["P2"].ToString();
        scoreTextP3.text = "P3: " + playerScores["P3"].ToString();
        scoreTextP4.text = "P4: " + playerScores["P4"].ToString();
    }

    // Método para actualizar la UI de la tarifa
    public void UpdateTarifaUI()
    {
        tarifaText.text = "Tarifa: " + tarifa.ToString();
    }
}
