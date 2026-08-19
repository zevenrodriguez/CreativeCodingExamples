using UnityEngine;
using UnityEngine.InputSystem;  // 1. The Input System "using" statement


public class MoveCharacter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction moveAction;

    CharacterController controller;
    float speed = 3.0f;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Debug.Log("Move Input: " + moveInput);
        Vector3 moveRight = transform.right * moveInput.x * speed;          
        Vector3 moveForward = transform.forward * moveInput.y * speed;

        controller.SimpleMove(moveForward + moveRight);

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Cube")
        {
            
        }
    }
}
