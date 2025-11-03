using UnityEngine;

public class DestroyBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float raycastDistance = 1.0f;
    [SerializeField] GameObject splashVFX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void FixedUpdate()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Floor"))
            {

                GameObject splash = Instantiate(splashVFX, transform.position, Quaternion.identity);
                splash.GetComponent<SplashDestroy>().DestroySplash();
                Destroy(gameObject);
            }
        }

        if (transform.position.y < -10.0f)
        {
            Destroy(gameObject);
        }
    }
}
