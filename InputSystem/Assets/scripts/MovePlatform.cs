using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float speed = 0.001f;
    [SerializeField] bool randomSpeed = false;
    [SerializeField] float screenLimit = -22.0f;
    [SerializeField] float resetPosition = 22.0f;


    void Start()
    {
        if (randomSpeed)
        {
            speed = Random.Range(0.001f, 0.01f);
        }
       
    }

    // Update is called once per frame
    void Update()
    {   
        float currentX = transform.position.x - speed;
        transform.position = new Vector3(currentX,transform.position.y, transform.position.z);
        if (transform.position.x < screenLimit)
        {
            transform.position = new Vector3(resetPosition, transform.position.y, transform.position.z);
        }
    }
}
