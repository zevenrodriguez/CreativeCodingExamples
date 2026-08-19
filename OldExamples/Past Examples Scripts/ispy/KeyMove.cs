using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMove : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 0.02f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.C)){
        Debug.Log(KeyCode.C + "Down");
       } 
       if(Input.GetKeyUp(KeyCode.C)){
        Debug.Log(KeyCode.C + "Up");
       }

       if(Input.GetKey(KeyCode.UpArrow)){
        Debug.Log(KeyCode.UpArrow);
        transform.position = transform.position + (Vector3.up * speed);
       }

       if(Input.GetKey(KeyCode.DownArrow)){
        Debug.Log(KeyCode.DownArrow);
        transform.position = transform.position + (Vector3.down * speed);
       }

       if(Input.GetKey(KeyCode.LeftArrow)){
        Debug.Log(KeyCode.LeftArrow);
        //transform.position = transform.position + (Vector3.up * speed);
        transform.position +=(Vector3.left * speed);
       }

       if(Input.GetKey(KeyCode.RightArrow)){
        Debug.Log(KeyCode.RightArrow);
        //transform.position = transform.position + (Vector3.up * speed);
        transform.position +=(Vector3.right * speed);
       }
    }
}
