using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GuessTheWord : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Button submit;
    [SerializeField] TMP_InputField entry;
    [SerializeField] TMP_Text[] output = new TMP_Text[4];
    char[] answer = new char[] { 'i', 'b', 'i', 's' };

    [SerializeField] TMP_Text incorrect;
    void Start()
    {
        StartCoroutine("StartTimer");
        submit.onClick.AddListener(() =>
        {
            StopCoroutine("StartTimer");
            StartCoroutine("StartTimer");
            Debug.Log(entry.text);
            Debug.Log(answer[1]);
            // for (int i=0;i<10;i++) {
            //     Debug.Log(i);
            // }

            bool correct = false;
            for (int i = 0; i < answer.Length; i++)
            {
                //Debug.Log(i);
                if (entry.text == answer[i].ToString())
                {
                    Debug.Log("Correct");
                    output[i].text = entry.text;
                    correct = true;
                }
            }

            if (correct == false)
            {
                incorrect.text = "incorrect";
                StartCoroutine("ClearText");
            }
            else
            {
                incorrect.text = "";
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ClearText()
    {
        Debug.Log("Before yield");
        yield return new WaitForSeconds(5.0f);
        Debug.Log("After yield");
        incorrect.text = "";

    }

    IEnumerator StartTimer()
    {
        incorrect.text = "10 Seconds Left";
        yield return new WaitForSeconds(10.0f);
        incorrect.text = "Times up";
    }
}
