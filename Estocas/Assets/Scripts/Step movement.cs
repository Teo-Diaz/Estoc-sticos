using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stepmovement : MonoBehaviour
{
    // Define the boundaries of the map
    private float minX = -1.1f;
    private float maxX = 2.2f;
    private float minY = -3.3f;
    private float maxY = 4.4f;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;

        if (Input.GetKeyDown(KeyCode.W))
            newPosition += new Vector3(0, 1.1f, 0);
        else if (Input.GetKeyDown(KeyCode.Q))
            newPosition += new Vector3(-1.1f, 1.1f, 0);
        else if (Input.GetKeyDown(KeyCode.A))
            newPosition += new Vector3(-1.1f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.D))
            newPosition += new Vector3(1.1f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.E))
            newPosition += new Vector3(1.1f, 1.1f, 0);

        // Clamp the new position to the boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Apply the new position
        transform.position = newPosition;
    }
}
