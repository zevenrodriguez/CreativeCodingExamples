using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sining : MonoBehaviour
{
    // Start is called before the first frame update
    float xPos = 0.0f;
    [SerializeField] float movement = 0.0f;
    [SerializeField] float distance = 0.01f;
    [SerializeField] float speed = 0.01f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.time);
        float deg = Time.time * Mathf.Rad2Deg;
        Debug.Log("Time.time: " + Time.time + " deg: " + deg + " sin: " + Mathf.Sin(deg));
        //This will output 1 radians are equal to 57.29578 degrees);
        movement = Mathf.Sin(deg * speed); 
        xPos= transform.position.x + (movement * Time.deltaTime) * distance;
		transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
}
