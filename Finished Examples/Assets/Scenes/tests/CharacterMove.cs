using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterMove : MonoBehaviour
{
     private float speed = 3.0f;
    private float rotationSpeed = 90.0f; // degrees per second
        
    private CharacterController characterController;

    InputAction moveAction;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
                moveAction = InputSystem.actions.FindAction("Move");

    }
        
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");
        
        // Rotate character
        transform.Rotate(Vector3.up, moveValue.x * rotationSpeed * Time.deltaTime);
        
        // Move character
        Vector3 moveDirection = transform.forward * moveValue.y * speed;
        
        characterController.SimpleMove(moveDirection);
    }
}
