using UnityEngine;
using UnityEngine.InputSystem;

// Put this on a Sphere that has a Rigidbody.
// Uses the standard actions in Assets/InputSystem_Actions ("Player" map):
//   Move (A/D, arrows, left stick) : push the ball left/right
//   Jump (Space, gamepad A)        : jump
// Tip: on the Rigidbody, tick Constraints > Freeze Position Z so the ball stays on the 2D line.
public class CharacterPlatformer : MonoBehaviour
{
    public float moveForce = 10f;  // how hard we push left/right
    public float jumpForce = 6f;   // how hard we push up

    Rigidbody rb;
    bool onGround = false;
    Vector3 startPosition;  // where we respawn

    InputAction moveAction;
    InputAction jumpAction;

    float currentPostionY = 0.0f;
    float previousPositionY = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;

        // Look up the actions by name.
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // Jump only if we are touching a block.
        if (jumpAction.WasPressedThisFrame() && onGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }
        previousPositionY = currentPostionY;
        currentPostionY = transform.position.y;

        if(onGround == false){
            
            if(currentPostionY < previousPositionY){
                GetComponent<Animator>().SetBool("jumping", false);
                GetComponent<Animator>().SetBool("falling", true);
            }else{
                GetComponent<Animator>().SetBool("jumping", true);
                GetComponent<Animator>().SetBool("falling", false);
            }
        }

        // Fell through a gap? Go back to the start.
        if (transform.position.y < -10f)
        {
            Respawn();
        }
    }

    // Physics forces go in FixedUpdate.
    void FixedUpdate()
    {
        // Move gives us a Vector2. We only need x: -1 = left, 1 = right.
        Vector2 move = moveAction.ReadValue<Vector2>();
        rb.AddForce(Vector3.right * move.x * moveForce);

        if (move.x < 0f && onGround == true)
        {
            // Flip the sprite to face left.
            GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<Animator>().Play("frogrun");
        }
        else if (move.x > 0f && onGround == true)
        {
            // Flip the sprite to face right.
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Animator>().Play("frogrun");

        }
        else if(move.x == 0f && onGround == true){
            GetComponent<Animator>().Play("frogidle");
        }
    }

    void Respawn()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector3.zero;   // stop falling
        rb.angularVelocity = Vector3.zero;  // stop spinning
    }

    // We landed on something, so we can jump again.
    void OnCollisionEnter(Collision collision)
    {
        onGround = true;
    }
}