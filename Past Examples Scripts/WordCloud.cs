using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordCloud : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button addWords;
    [SerializeField] Button generateCloud;
    [SerializeField] TMP_InputField textInput;

    List<TMP_Text> cloud = new List<TMP_Text>();

    [SerializeField] TMP_Text cloudText;

    void Start()
    {
        addWords.onClick.AddListener(()=>{
            Debug.Log(textInput.text);
            cloudText.text = textInput.text;
            cloudText.color = new Color(Random.Range(0,100),Random.Range(0,255),Random.Range(100,255));
            float posX = Random.Range(-5,5);
            float posY = Random.Range(-5,5);
            float posZ = Random.Range(0,5);
            cloud.Add(Instantiate(cloudText, new Vector3(posX,posY,posZ), Quaternion.identity));
            foreach(TMP_Text curText in cloud){
                curText.fontSize = Random.Range(5,10);
                curText.color = new Color(Random.Range(0,100),Random.Range(0,255),Random.Range(100,255));
            }
        });

        generateCloud.onClick.AddListener(()=>{
            foreach(TMP_Text curText in cloud){
                curText.color = new Color(Random.Range(0,100),Random.Range(0,255),Random.Range(100,255));
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
