using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    bool rotateCharacter = false;
    [SerializeField] Animator anim;
    bool jump = false;
    [SerializeField] Rigidbody rb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       float posX = transform.position.x + (Input.GetAxis("Horizontal") * Time.deltaTime);
       transform.position = new Vector3(posX, transform.position.y, transform.position.z);

       if(Input.GetAxis("Horizontal") < 0){
        //left
        anim.SetTrigger("run");
        if(rotateCharacter == true){
            transform.Rotate(0,180,0,Space.Self);
            rotateCharacter = false;
        }
       }

       if(Input.GetAxis("Horizontal") > 0){
        //right
        anim.SetTrigger("run");
        if(rotateCharacter == false){
            transform.Rotate(0,-180,0,Space.Self);
            rotateCharacter = true;
        }
       } 

       if(Input.GetKeyDown(KeyCode.Space)){
            jump = true;
       }
    }

    void FixedUpdate(){
        if(jump == true){
            jump = false;
            rb.AddForce(new Vector3(0,5,0),ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.name == "Cube"){
            Debug.Log("Grounded");
            anim.SetBool("isGrounded", true);
        }
    }
}
