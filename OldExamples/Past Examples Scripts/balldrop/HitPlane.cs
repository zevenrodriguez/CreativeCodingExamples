using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlane : MonoBehaviour
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
    // void FixedUpdate(){
    //     RaycastHit hit;
    //     if(Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f)){
    //         Debug.Log("Hit Plane");
    //     }
    // }
    void OnCollisionEnter(Collision col){
        Debug.Log(col.gameObject.tag);
        if(col.gameObject.CompareTag("cannonball")){
            curPart.transform.position = col.gameObject.transform.position;
            StartCoroutine("partStartStop", 0.25f);
            Destroy(col.gameObject);
        }
    }
    IEnumerator partStartStop(float splashTime){
        curPart.Play();
        yield return new WaitForSeconds(splashTime);
        curPart.Stop(true);
    }
}
