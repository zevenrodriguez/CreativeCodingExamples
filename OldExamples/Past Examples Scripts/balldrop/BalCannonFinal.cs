using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalCannonFinal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject cannonball;
    [SerializeField] Transform spawnPoint;
    GameObject currentBall;
    Rigidbody curRB;
    bool launch = false;
    [SerializeField] float speed = 10.0f;
     
    void Start()
    {
    //    Vector3 cbPos = new Vector3(0.0f,3.0f,0.0f);
    //    currentBall = Instantiate(cannonball,spawnPoint.position,Quaternion.identity); 
    //     curRB = currentBall.gameObject.GetComponent<Rigidbody>();
        //curRB.AddForce(Vector3.forward * 10, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.Space)){
            currentBall = Instantiate(cannonball,spawnPoint.position,Quaternion.identity); 
            curRB = currentBall.gameObject.GetComponent<Rigidbody>();
         }
         if (Input.GetKey(KeyCode.Space))
        {
            launch = true;
        }else{
            launch = false;
        }
    }

    void FixedUpdate(){
        if(launch == true){
            curRB.AddForce(Vector3.forward * speed, ForceMode.Force);
        }
    }
}
