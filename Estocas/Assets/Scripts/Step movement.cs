using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Importar para trabajar con botones UI

public class Stepmovement : MonoBehaviour
{
    // Define the boundaries of the map
    private float minX = -1.1f;
    private float maxX = 2.2f;
    private float minY = -3.3f;
    private float maxY = 4.4f;

    // Lista de los objetos que van a moverse
    public List<Transform> Players;

    // Índice del objeto que tiene el turno
    private int currentObjectIndex = 0;

    // Booleano para controlar si los jugadores pueden moverse
    private bool canMove = true;

    // Referencia al botón de la UI
    public Button resetButton;

    void Start()
    {
        // Asignar una función al botón UI para cuando sea presionado
        resetButton.onClick.AddListener(ResetTurns);
    }

    // Update is called once per frame
    void Update()
    {
        // Solo permite mover si es el turno de los jugadores y no se han movido los 4
        if (canMove)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        // Detectar entrada del usuario para mover al objeto actual
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            // Obtener el objeto actual (el que tiene el turno)
            Transform currentObject = Players[currentObjectIndex];
            Vector3 newPosition = currentObject.position;

            // Detectar qué tecla fue presionada y mover en consecuencia
            if (Input.GetKeyDown(KeyCode.W))
                newPosition += new Vector3(0, 1.1f, 0);
            else if (Input.GetKeyDown(KeyCode.Q))
                newPosition += new Vector3(-1.1f, 1.1f, 0);
            else if (Input.GetKeyDown(KeyCode.E))
                newPosition += new Vector3(1.1f, 1.1f, 0);

            // Asegurarse de que el objeto no se salga de los límites
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            // Aplicar la nueva posición al objeto actual
            currentObject.position = newPosition;

            // Cambiar de turno al siguiente objeto
            NextTurn();
        }
    }

    void NextTurn()
    {
        // Pasar al siguiente objeto en la lista
        currentObjectIndex = (currentObjectIndex + 1) % Players.Count;

        // Si el turno vuelve al primero (todos los objetos ya se movieron), deshabilitar el movimiento
        if (currentObjectIndex == 0)
        {
            canMove = false;  // Desactivar movimiento hasta que se presione el botón
        }
    }

    // Función que se llamará al presionar el botón de la UI
    void ResetTurns()
    {
        canMove = true;  // Permitir que los jugadores vuelvan a moverse
    }
}
