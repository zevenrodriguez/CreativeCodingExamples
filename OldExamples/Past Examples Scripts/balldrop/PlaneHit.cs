using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneHit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ParticleSystem curPart;
    void Start()
    {
        curPart.Stop(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate(){
        // RaycastHit hit;
        // if(Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f)){
        //     if(hit.collider.gameObject.tag == "cannonball"){
        //         Debug.Log("hit plane");
        //         StartCoroutine("ParticleStartStop", 2.0f);
        //         Destroy(hit.collider.gameObject);

        //     }
            
        // }
    }
    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "cannonball"){
                Debug.Log("hit plane");
                curPart.transform.position = col.gameObject.transform.position;
                StartCoroutine("ParticleStartStop", 2.0f);
                Destroy(col.gameObject);
            }
    }
    public IEnumerator ParticleStartStop(float splashTime){
        curPart.Play();
        yield return new WaitForSeconds(splashTime);
        curPart.Stop(true);
    }
}
