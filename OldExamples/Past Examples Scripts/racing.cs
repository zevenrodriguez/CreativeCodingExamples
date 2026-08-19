using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class racing : MonoBehaviour
{
    /* 
    multi line comments
    This program simulates a race between a cube and sphere
    
     */
    [SerializeField] Button startButton;
    [SerializeField] TMP_Text winner;
    [SerializeField] GameObject cube;
    [SerializeField] GameObject sphere;
    bool startRacing = false;
    float cubeXSpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartRace);
    }

    // Update is called once per frame
    void Update()
    {
        if(startRacing == true){
            //move the cube and sphere
            //Debug.Log("Race Started");
            //float cubeXPos = cube.gameObject.transform.position.x + 0.01f;
            cubeXSpeed = Random.Range(-0.01f,0.02f);
            Debug.Log(cubeXSpeed);
            float cubeXPos = cube.gameObject.transform.position.x + cubeXSpeed;
            cube.gameObject.transform.position = new Vector3(cubeXPos,2.0f,0.0f);

            if(cube.gameObject.transform.position.x > 8){
                //cube wins
                winner.text = "Cube Wins!";
                startRacing = false;
            }
        }
    }

    void StartRace(){
        //change variable that enables objects to move
        startRacing = true;
        // cubeXSpeed = Random.Range(0.001f,0.02f);
        // Debug.Log(cubeXSpeed);
    }
}
