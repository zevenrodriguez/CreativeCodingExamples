using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class CMThirdPersonMove : MonoBehaviour
{
      // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction moveAction;
    [SerializeField] float speed = 5.0f;
    [SerializeField] Canvas canvas;
    [SerializeField] TMP_Text outputText;
    private CharacterController ourCharacterController;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        canvas.enabled = false;
        ourCharacterController = GetComponent<CharacterController>();


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //Debug.Log("Move Value: " + moveValue + " Move X: " + moveValue.x + " Move Y: " + moveValue.y);
        float moveDistance = moveValue.y * Time.deltaTime * speed;
        float rotateYAxis = moveValue.x * Time.deltaTime * speed;
        Debug.Log(rotateYAxis);
        transform.Rotate(0f, rotateYAxis, 0f);
        ourCharacterController.Move(transform.forward * moveDistance);
       
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Interactable0")
        {
            Debug.Log("Collided with Interactable0");
            canvas.enabled = true;
            outputText.text = "Collided with Interactable0";
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Interactable0")
        {
            Debug.Log("Exited collision with Interactable0");
            canvas.enabled = false; 
            outputText.text = "";    
        }
    }
}
