using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stepmovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            transform.Translate(0, 1.1f, 0);
        else if (Input.GetKeyDown(KeyCode.Q))
            transform.Translate(-1.1f, 1.1f, 0);
        else if (Input.GetKeyDown(KeyCode.A))
            transform.Translate(-1.1f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.D))
            transform.Translate(1.1f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.E))
            transform.Translate(1.1f, 1.1f, 0);
    }
}
    