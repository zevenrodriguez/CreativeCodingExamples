using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroy : MonoBehaviour
{
    void FixedUpdate(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f)){
            if(hit.collider.gameObject.name == "Plane"){
                Debug.Log("hit plane");
                Destroy(this.gameObject);
            }
            
        }
        if(transform.position.y < -20){
            Debug.Log("into the abyss");
            Destroy(this.gameObject);
        }
    }
}
