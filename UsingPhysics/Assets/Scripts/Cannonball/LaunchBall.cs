using UnityEngine;
using UnityEngine.InputSystem;

// A simple cannon that uses the standard actions in Assets/InputSystem_Actions ("Player" map):
//   Move     (WASD / arrows / left stick)       : turn and tilt the cannon
//   Attack   (left mouse / Enter / gamepad X)   : FIRE!
//   Next     (2 / d-pad right)                  : more power
//   Previous (1 / d-pad left)                   : less power
public class LaunchBall : MonoBehaviour
{
    [Header("What to shoot")]
    public GameObject cannonball;   // the cannonball prefab
    public Transform barrel;        // the cannon (a cylinder: its long side is its "up" direction)

    [Header("Power")]
    public float launchSpeed = 15f;     // how fast the ball leaves the barrel
    public float minSpeed = 5f;
    public float maxSpeed = 30f;
    public float speedChangeRate = 10f; // how quickly the power changes

    [Header("Aiming")]
    public float aimSpeed = 60f;  // degrees per second
    public float minAngle = 10f;  // lowest tilt (0 = flat)
    public float maxAngle = 80f;  // highest tilt (90 = straight up)

    // Actions from the "Player" action map.
    InputAction moveAction;
    InputAction attackAction;
    InputAction nextAction;
    InputAction previousAction;

    float tiltAngle = 45f;  // how far the barrel is tilted up from the ground
    float turnAngle = 0f;   // how far the cannon is turned left or right

    void Start()
    {
        // Look up the actions by name.
        moveAction     = InputSystem.actions.FindAction("Move");
        attackAction   = InputSystem.actions.FindAction("Attack");
        nextAction     = InputSystem.actions.FindAction("Next"); // Press 1
        previousAction = InputSystem.actions.FindAction("Previous"); // Press 2
    }

    void Update()
    {
        // --- Aim ---
        // Move gives us a Vector2: x = left/right, y = up/down (each between -1 and 1).
        Vector2 move = moveAction.ReadValue<Vector2>();
        turnAngle += move.x * aimSpeed * Time.deltaTime;
        tiltAngle += move.y * aimSpeed * Time.deltaTime;

        // Keep the tilt between the min and max angle.
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);

        // A cylinder stands straight up by default, so we lean it forward by (90 - tilt).
        barrel.rotation = Quaternion.Euler(90f - tiltAngle, turnAngle, 0f);

        // --- Power ---
        if (nextAction.IsPressed())
        {
           launchSpeed += speedChangeRate * Time.deltaTime; 
        }    
        if (previousAction.IsPressed())
        {
          launchSpeed -= speedChangeRate * Time.deltaTime;  
        } 

        launchSpeed = Mathf.Clamp(launchSpeed, minSpeed, maxSpeed);

        // --- Fire ---
        if (attackAction.WasPressedThisFrame())
        {
            Fire();
        }
    }

    void Fire()
    {
        // The direction the barrel is pointing.
        Vector3 direction = barrel.up;

        // Start the ball just past the end of the barrel.
        Vector3 startPosition = barrel.position + direction * 1.5f;

        GameObject ball = Instantiate(cannonball, startPosition, Quaternion.identity);

        // Push it out of the barrel. Gravity takes care of the arc!
        ball.GetComponent<Rigidbody>().linearVelocity = direction * launchSpeed;

        Debug.Log("Fired at " + tiltAngle.ToString("0") + " degrees with speed " + launchSpeed.ToString("0.0"));
    }
}
