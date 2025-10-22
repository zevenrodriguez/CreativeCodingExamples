using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchBall : MonoBehaviour
{
    [SerializeField] GameObject cannonball;
    [SerializeField] Transform spawnPoint;
    GameObject currentBall;
    bool launch = false;
    [SerializeField] float speed = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction launchAction;

    bool wasPressed = false;
    void Start()
    {
        launchAction = InputSystem.actions.FindAction("Launch");
    }

    // Update is called once per frame
    void Update()
    {
        if (launchAction.IsPressed() == true)
        {
            //Debug.Log("Launch");  
            wasPressed = true;
        }
        else
        {
            if(wasPressed == true)
            {
                wasPressed = false;
                Debug.Log("Launch");
                
            }
        }
    }
}
