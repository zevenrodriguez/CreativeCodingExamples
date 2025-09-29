using UnityEngine;
using UnityEngine.InputSystem;  


public class CharacterMove : MonoBehaviour
{
    InputAction moveAction;
    float speed = 3.0f;
    float rotationSpeed = 90.0f; // degrees per second

    CharacterController characterController;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");

    }

    void Update()
    {
  
        Vector2 moveValue = moveAction.ReadValue<Vector2>();


        // Rotate character
        transform.Rotate(Vector3.up, moveValue.x * rotationSpeed * Time.deltaTime);

        // Move character
        Vector3 moveDirection = transform.forward * moveValue.y * speed;
        
        characterController.SimpleMove(moveDirection);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);
    }
}