using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput : MonoBehaviour
{
    // Start is called before the first frame update
    float hInput = 0.0f;
    float vInput = 0.0f;
    float speed = 0.02f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        //Debug.Log("vInput: " + vInput + " hInput: " + hInput);

        float posX = transform.position.x + (hInput * speed);
        float posY = transform.position.y + (vInput * speed);



        transform.position = new Vector3(posX,posY,-10);
    }
}
