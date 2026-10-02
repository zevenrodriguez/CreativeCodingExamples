using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CMTopMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction moveAction;
    [SerializeField] float speed = 5.0f;
    [SerializeField] Canvas canvas;
    [SerializeField] TMP_Text outputText;

    string[] ObjectsFound;

    int counter = 0;
    [SerializeField] PlaceObjects amount;  



    private CharacterController ourCharacterController;
    void Start()
    {
    moveAction = InputSystem.actions.FindAction("Move");
    canvas.enabled = false;
    ourCharacterController = GetComponent<CharacterController>();
    ObjectsFound = new string[amount.numberOfObjects];

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //Debug.Log("Move Value: " + moveValue + " Move X: " + moveValue.x + " Move Y: " + moveValue.y);
        Vector3 movement = new Vector3(moveValue.x, 0, moveValue.y);
        ourCharacterController.Move(movement * speed * Time.deltaTime);

    }

    void OnTriggerEnter(Collider col)
    {
        // if (col.gameObject.name == "Interactable0")
        // {
        //     Debug.Log("Collided with Interactable0");
        //     canvas.enabled = true;
        //     outputText.text = "Collided with Interactable0";
        // }
        Debug.Log("Collided with object with tag: " + col.gameObject.tag);


        if (col.gameObject.tag == "cube" || col.gameObject.tag == "sphere" || col.gameObject.tag == "cylinder")
        {
            AddItem(col.gameObject.tag);
            Destroy(col.gameObject);
            bool allFound = CheckIfAllObjectsFound();
            if (allFound == true)
            {
                Debug.Log("All objects found!");
                canvas.enabled = true;
                outputText.text = "All objects found!";
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        // if (col.gameObject.name == "Interactable0")
        // {
        //     Debug.Log("Exited collision with Interactable0");
        //     canvas.enabled = false; 
        //     outputText.text = "";    
        // }
    }

    void AddItem(string item)
    {
        ObjectsFound[counter] = item;
    }
    bool CheckIfAllObjectsFound()
    {
        counter = counter + 1;
        if (counter >= ObjectsFound.Length)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
