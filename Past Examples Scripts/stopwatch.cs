using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class stopwatch : MonoBehaviour
{
    // Start is called before the first frame update
    int number = 5; //this creates a comment, int represents a whole number
    float decimalNum = 123.5f; //this is a decimal number
    string words = "These are words";
    char letter = 'a';
    bool answer = true;

    [SerializeField] TMP_Text timeOutput; // camelCase 
    //public TMP_Text timeOutput2;

    [SerializeField] Button startButton;

    float currentTime = 0.0f;
    bool timerState = false;
    void Start()
    {
        Debug.Log("The script has started");
        startButton.onClick.AddListener(StartTime);
    }

    // Update is called once per frame
    void Update()
    {
        //working with integers
        // number = number + 1;
        // Debug.Log("Number: " + number);
        // timeOutput.text = number.ToString();
        // "1" is not the same as 1
        // decimalNum = decimalNum + 0.1f;
        // Debug.Log("Decimal Num: " + decimalNum);
        // timeOutput.text = decimalNum.ToString();
       //Debug.Log("The script is in the update loop"); 
        if(timerState == true){
            //increase time
            currentTime = currentTime + Time.deltaTime;
        }
       
       //Debug.Log("currentTime: " + currentTime + " deltaTime: " + Time.deltaTime);
       //timeOutput.text = currentTime.ToString();
       int convertTime = (int)currentTime;
       timeOutput.text = convertTime.ToString();

    }

    void StartTime(){
        Debug.Log("StartTime");
        timerState = true;
    }
}
