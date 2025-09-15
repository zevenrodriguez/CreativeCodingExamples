using UnityEngine;
using UnityEngine.InputSystem;  // 1. The Input System "using" statement
public class MovePlayer : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    Rigidbody rb;
    bool jump = false;

    public float jumpForce = 150f;

    bool cantJump = false;
    void Start()
    {

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //Debug.Log("Move: " + moveValue);
        float posX = transform.position.x + (moveValue.x * Time.deltaTime);
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);

        if (jumpAction.IsPressed() && cantJump == false)
        {
            // your jump code here
            Debug.Log("Jump");
            jump = true;
        }



    }

    void FixedUpdate()
    {
        if (jump == true)
        {
            jump = false;
            rb.AddForce((jumpForce * transform.up), ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Floor")
        {
            // Debug.Log("Grounded");
            // anim.SetBool("isGrounded", true);
            cantJump = false;
        }
    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name == "Floor")
        {
            // Debug.Log("Not Grounded");
            // anim.SetBool("isGrounded", false);
            cantJump = true;
        }
    }


}
