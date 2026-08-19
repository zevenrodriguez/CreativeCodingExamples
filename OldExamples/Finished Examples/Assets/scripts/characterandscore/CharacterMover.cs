using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class CharacterMover : MonoBehaviour
{
    InputAction charMove;
    CharacterController characterController;

    [SerializeField] ScoreUpdate score;
    [SerializeField] AudioManager audioManager;

    float speed = 10.0f;
    float rotationSpeed = 90.0f;

    [SerializeField] Canvas message;

    void Start()
    {
        charMove = InputSystem.actions.FindAction("Move");
        characterController = GetComponent<CharacterController>();
        message.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = charMove.ReadValue<Vector2>();
        Vector3 moveDirection = transform.forward * moveValue.y * speed;

        transform.Rotate(Vector3.up, moveValue.x * rotationSpeed * Time.deltaTime);

        characterController.SimpleMove(moveDirection);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.gameObject.name);
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.gameObject.name);
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "good")
        {
            //increase score
            int curIncrease = Random.Range(0, 3);
            score.increaseScore(curIncrease);
            score.updateText();
            audioManager.playClip("good");
            message.enabled = true;
        }
        else if (col.gameObject.tag == "bad")
        {
            //decrease score 
            int curDecrease = Random.Range(0, 3);
            score.decreaseScore(curDecrease);
            score.updateText();
            audioManager.playClip("bad");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "good") {
            message.enabled = false;
        }
    }
}
