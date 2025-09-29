using UnityEngine;
using UnityEngine.InputSystem;
public class MovePlayer : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    bool jump = false;
    //[SerializeField] Rigidbody rb;
    Rigidbody rb;

    [SerializeField] float forceAmount = 150.0f;

    bool canJump = false;

    //[SerializeField] Transform cam;
    [SerializeField] float speed = 2.0f;

    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer bunny;

    float prevPosY = 0.0f;

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
        //Debug.Log("moveValue " + moveValue);
        float posX = transform.position.x + (moveValue.x * (Time.deltaTime * speed));
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        //cam.position = new Vector3(posX, cam.position.y, cam.position.z);

        if (moveValue.x < 0)
        {
            //Running Left
            anim.Play("run");
            bunny.flipX = false;
        }
        else if (moveValue.x > 0)
        {
            //Running Right
            anim.Play("run");
            bunny.flipX = true;
        }
        else
        {
            //anim.Play("idle");
        }


        if (jumpAction.IsPressed() == true && canJump == true)
        {
            Debug.Log("Jumping");
            jump = true;
            //anim.Play("jump");
        }

        if (transform.position.y < -12)
        {
            transform.position = new Vector3(0.0f, 10.0f, 0.0f);
            //after x amount of times game over
            //game over
        }


        if (transform.position.y > prevPosY)
        {
            prevPosY = transform.position.y;
            Debug.Log("going up");
            anim.SetBool("falling", false);
        }

        if (transform.position.y < prevPosY)
        {
            prevPosY = transform.position.y;
            Debug.Log("falling");
            anim.SetBool("falling", true);
        }



        
    }

    void FixedUpdate()
    {
        if (jump == true)
        {
            jump = false;
            rb.AddForce((forceAmount * transform.up), ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision col)
    {

        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Floor")
        {
            canJump = true;
            anim.Play("idle");
        }
    }

    void OnCollisionExit(Collision col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Floor")
        {
            canJump = false;
            anim.Play("jump");
        }
    }
}
