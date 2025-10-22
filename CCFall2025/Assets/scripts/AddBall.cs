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

    InputAction lookAction;
    void Start()
    {
        // ballPosition.x = Random.Range(-5.0f, 5.0f);
        // ballPosition.y = Random.Range(1.0f, 6.0f);
        // ballPosition.z = Random.Range(-5.0f,5.0f);
        // Instantiate(ball,ballPosition,Quaternion.identity);
        //BallAdd();
        StartCoroutine("CreateBall", 3.0f);
        lookAction = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Mouse.current.position.x.ReadValue();
        float mouseY = Mouse.current.position.y.ReadValue();
        
        Vector3 test = new Vector3(mouseX,mouseY,0.0f);
        Ray cammouseray = cam.ScreenPointToRay(test);
        Debug.DrawRay(cammouseray.origin, cammouseray.direction * 10, Color.yellow);

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
