using UnityEngine;

public class ForceRacing : MonoBehaviour
{
    [SerializeField] Rigidbody player;
    
    [SerializeField] float speed = 10.0f;
        bool launch = false;
     
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
        
         if (Input.GetKey(KeyCode.Space))
        {
            launch = true;
        }else{
            launch = false;
        }
    }

    void FixedUpdate(){
        if(launch == true){
            player.AddForce(Vector3.left * speed, ForceMode.Force);
        }
    }
}
