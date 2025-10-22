using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuessTheWord : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button submit;
    [SerializeField] TMP_InputField entry;
    [SerializeField] TMP_Text[] letters = new TMP_Text[4];
    char[] answer = new char[]{'i','b','i','s'};
    //string[] answer = new char[]{"i","b","i","s"};\
    [SerializeField] TMP_Text rightWrong;
    bool ci = false;

    void Start()
    {
        for(int i = 0; i<10; i++){
            Debug.Log(i);
        }
        StartCoroutine("StartTimer");
        submit.onClick.AddListener(()=>{
            StopCoroutine("StartTimer");
            StartCoroutine("StartTimer");
            Debug.Log(entry.text);
            for(int i = 0; i< answer.Length; i++){
                if(entry.text == answer[i].ToString()){
                    Debug.Log("match");
                    letters[i].text = answer[i].ToString();
                    ci = true;
                    //rightWrong.text = "Correct!";
                }else{
                    Debug.Log("Not match found");
                    //rightWrong.text = "incorrect!";

                }
            }

            if(ci == true){
                rightWrong.text = "Correct!";
            }else{
                rightWrong.text = "incorrect!";
            }

            ci = false;
            StartCoroutine("ClearText");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ClearText(){
        yield return new WaitForSeconds(5.0f);
        rightWrong.text = "";
    }
    IEnumerator StartTimer(){
        rightWrong.text = "10 seconds left";
        yield return new WaitForSeconds(10.0f);
        rightWrong.text = "Times up";
        //submit.gameObject.SetActive(false);
        submit.interactable = false;

    }
}
