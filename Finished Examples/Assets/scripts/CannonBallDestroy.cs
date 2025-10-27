using UnityEngine;



public class CannonBallDestroy : MonoBehaviour
{
    [SerializeField] float raycastDistance = 1.0f; // Maximum distance to check for ground
    [SerializeField] GameObject splashVFX;
    
    bool destroyObjects = false;

    void Update()
    {
       
    }

    void FixedUpdate()
    {
        // Create a raycast pointing downward from the object's position
        RaycastHit hit;

        // Draw debug ray (visible in Scene view)
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // If we hit something and it has the "Floor" tag (make sure your plane has this tag)
            if (hit.collider.CompareTag("Floor"))
            {
                // Destroy this gameObject
                Debug.Log("Cannonball hit the floor, playing splash effect and destroying the cannonball.");
                GameObject splash = Instantiate(splashVFX, transform.position, Quaternion.identity);
                splash.GetComponent<SplashDestroy>().DestroySplash();

                Destroy(gameObject);
               
              
            }
        }

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

}
