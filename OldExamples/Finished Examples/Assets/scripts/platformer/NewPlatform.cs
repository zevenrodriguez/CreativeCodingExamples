using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPlatform : MonoBehaviour
{

    [SerializeField] GameObject platform;
    [SerializeField] GameObject shortPlatform;

    void Start()
    {
        StartCoroutine("CreatePlatform", 10.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CreatePlatform(float interval)
    {
        //before
        Debug.Log("Before");
        float randNumber = Random.Range(0.0f,1.0f);
        GameObject curObject;
        if (randNumber < 0.5)
        {
            curObject = platform;
            Debug.Log("Long");
        }
        else
        {
            curObject = shortPlatform;
            Debug.Log("short");
        }
        //Instantiate(platform, transform.position, Quaternion.identity);
        Instantiate(curObject, transform.position, Quaternion.identity);

        //wait
        yield return new WaitForSeconds(interval);

        //after
        Debug.Log("After");
        StartCoroutine("CreatePlatform", 10.0f);


    }
    

}
