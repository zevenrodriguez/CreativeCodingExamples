using UnityEngine;
using UnityEngine.InputSystem;


public class LaunchBall : MonoBehaviour
{
    [SerializeField] GameObject cannonball;
    [SerializeField] Transform spawnPoint;
    GameObject currentBall;
    bool launch = false;
    [SerializeField] float speed = 10.0f;

    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float minSpeed = 5.0f;
    [SerializeField] float cannonSpeed = 2.0f;
    [SerializeField] float rotationSpeed = 90.0f; // degrees per second when using move.y
    [SerializeField] float minYaw = 45.0f;
    [SerializeField] float maxYaw = 90f;

    // track yaw explicitly to avoid Euler angle wrapping/jumps
    float yaw;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction launchAction;
    InputAction moveAction;




    bool wasPressed = false;
    void Start()
    {
        launchAction = InputSystem.actions.FindAction("Launch");
        moveAction = InputSystem.actions.FindAction("Move");

        // initialize tracked yaw to the spawnPoint's current Y rotation
        yaw = spawnPoint.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float posX = spawnPoint.position.x + (moveValue.x * (Time.deltaTime * cannonSpeed));
        spawnPoint.position = new Vector3(posX, spawnPoint.position.y, spawnPoint.position.z);

        // Use moveValue.y to rotate the spawnPoint around the Y axis
        // Update tracked yaw (prevents jumps/wrapping issues from directly reading eulerAngles)
        yaw = yaw + (moveValue.y * rotationSpeed * Time.deltaTime);
        // Debug.Log("Yaw before clamp: " + yaw);
        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        //Mathf.Clamp simply limits a value to a minimum and maximum — if the value is below the min it returns the min, if it’s above the max it returns the max, otherwise it returns the value unchanged.
        // Debug.Log("Yaw after clamp: " + yaw);
        spawnPoint.rotation = Quaternion.Euler(yaw, spawnPoint.eulerAngles.y, spawnPoint.eulerAngles.z);


        speed = Map(yaw, minYaw, maxYaw, maxSpeed, minSpeed);


        if (launchAction.WasReleasedThisFrame() == true)
        {
            Debug.Log("Launch");
            if (currentBall == null)
            {
                currentBall = Instantiate(cannonball, spawnPoint.position, Quaternion.identity);

            }
        }

    }

    private void FixedUpdate()
    {
        if (currentBall != null)
        {
            Rigidbody rb = currentBall.GetComponent<Rigidbody>();

            rb.AddForce(new Vector3(0, 1, 1) * speed, ForceMode.Impulse);
            currentBall = null;
        }
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
