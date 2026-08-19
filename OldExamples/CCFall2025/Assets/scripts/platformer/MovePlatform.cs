using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] float speed = 0.01f;
    [SerializeField] bool randomSpeed = false;
    [SerializeField] float screenLimit = -22.0f;
    [SerializeField] float resetPosition = 22.0f;    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float posX = transform.position.x - speed;
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        if (transform.position.x < screenLimit)
        {
            //transform.position = new Vector3(resetPosition, transform.position.y, transform.position.z);
            Destroy(this.gameObject);
        }
    }
}
