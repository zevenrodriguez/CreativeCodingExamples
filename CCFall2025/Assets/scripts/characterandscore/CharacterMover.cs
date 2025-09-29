using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterMover : MonoBehaviour
{
    InputAction charMove;
    CharacterController characterController;

    float speed = 10.0f;
    float rotationSpeed = 90.0f;

    void Start()
    {
        charMove = InputSystem.actions.FindAction("Move");
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = charMove.ReadValue<Vector2>();
        Vector3 moveDirection = transform.forward * moveValue.y * speed;

        transform.Rotate(Vector3.up, moveValue.x * rotationSpeed * Time.deltaTime);

        characterController.SimpleMove(moveDirection);
    }
}
