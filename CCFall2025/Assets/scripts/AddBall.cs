using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class AddBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject ball;
    Vector3 ballPosition = new Vector3();

    [SerializeField] Camera cam;

    void Start()
    {
        // ballPosition.x = Random.Range(-5.0f, 5.0f);
        // ballPosition.y = Random.Range(1.0f, 6.0f);
        // ballPosition.z = Random.Range(-5.0f,5.0f);
        // Instantiate(ball,ballPosition,Quaternion.identity);
        //BallAdd();
        StartCoroutine("CreateBall", 3.0f);
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
            Debug.Log(hit.collider.gameObject.name);
            //hit.transform.GetComponent<Renderer>().material.color = Color.red;
            if (hit.collider.gameObject.tag == "ball" && Mouse.current.leftButton.wasPressedThisFrame == true)
            {
              Destroy(hit.collider.gameObject);  
            }
            
        }


    }

    void BallAdd()
    {
        ballPosition.x = Random.Range(-5.0f, 5.0f);
        ballPosition.y = Random.Range(1.0f, 6.0f);
        ballPosition.z = Random.Range(-5.0f, 5.0f);
        Instantiate(ball, ballPosition, Quaternion.identity);
    }
    
    IEnumerator CreateBall(float interval)
    {
        yield return new WaitForSeconds(interval);
        BallAdd();
        StartCoroutine("CreateBall",3.0f);
    }
}
