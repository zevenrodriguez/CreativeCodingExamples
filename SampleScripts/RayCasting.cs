using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class RayCasting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] Camera cam;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Mouse.current.position.x.ReadValue();
        float mouseY = Mouse.current.position.y.ReadValue();
        
        Vector3 mousePosition = new Vector3(mouseX,mouseY,0.0f);
        Ray camMouseRay = cam.ScreenPointToRay(mousePosition);
        Debug.DrawRay(camMouseRay.origin, camMouseRay.direction * 10, Color.yellow);
        if (Physics.Raycast(camMouseRay, out RaycastHit hit))
        {
            if (Mouse.current.leftButton.wasPressedThisFrame == true)
            {
                Debug.Log(hit.collider.gameObject.name);
            }
            
        }


    }

}
