using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -20){
            Destroy(this.gameObject);
            Debug.Log("Into the abyss");
        }
    }

    // void FixedUpdate(){
    //     RaycastHit hit;
    //     if(Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f)){
    //         if(hit.collider.gameObject.name == "Plane"){
    //             Debug.Log("hit plane");
    //             //part.GetComponent<ParticleSystem>().Play();
    //             //StartCoroutine(part.ParticleStartStop(2.0f));
    //             //Destroy(this.gameObject);

    //         }
            
    //     }
    // }
}
