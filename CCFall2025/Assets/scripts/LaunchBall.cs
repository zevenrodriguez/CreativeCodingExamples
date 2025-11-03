using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchBall : MonoBehaviour
{
    [SerializeField] GameObject cannonball;
    [SerializeField] Transform spawnPoint;
    GameObject currentBall;
    bool launch = false;
    [SerializeField] float speed = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    InputAction launchAction;
    InputAction moveAction;

    [SerializeField] float cannonSpeed = 2.0f;

    [SerializeField] float rotationSpeed = 90.0f;
    [SerializeField] float minYaw = 45.0f;
    [SerializeField] float maxYaw = 90.0f;

    float yaw = 0.0f;

    void Start()
    {
        launchAction = InputSystem.actions.FindAction("Launch");
        moveAction = InputSystem.actions.FindAction("Move");

        yaw = spawnPoint.eulerAngles.x;
        Debug.Log(yaw);

    }

    // Update is called once per frame
    void Update()
    {
        //yaw = spawnPoint.eulerAngles.x;
        //Debug.Log(yaw);
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //left and right movement
        float posX = spawnPoint.position.x + (moveValue.x * (Time.deltaTime * cannonSpeed));
        spawnPoint.position = new Vector3(posX, spawnPoint.position.y, spawnPoint.position.z);
        // up and down rotation

        yaw = yaw + (moveValue.y * rotationSpeed * Time.deltaTime);
        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

        spawnPoint.rotation = Quaternion.Euler(yaw,spawnPoint.eulerAngles.y, spawnPoint.eulerAngles.z);
        speed = Map(yaw, 45.0f, 90.0f, 20.0f, 5.0f);
        
        if (launchAction.WasReleasedThisFrame() == true)
        {
            if (currentBall == null)
            {
                //if currentBall is empty
                currentBall = Instantiate(cannonball, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    void FixedUpdate()
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
