using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class magic8ball : MonoBehaviour
{
    
    [SerializeField] Button ShakeButton;
    [SerializeField] TMP_Text outputText;

    [SerializeField] Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShakeButton.onClick.AddListener(() => {
            animator.Play("shake");
            int randomNumber = Random.Range(0, 6);
            //Random.Range is exclusive of the max value
            Debug.Log(randomNumber);
            chooseAnswer(randomNumber);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void chooseAnswer(int index)
    {
        if(index == 0)
        {
            outputText.text = "Yes";
        }
        else if(index == 1)
        {
            outputText.text = "No";
        }
        else if(index == 2)
        {
            outputText.text = "Maybe";
        }
        else if(index == 3)
        {
            outputText.text = "Ask again later";
        }
        else if(index == 4)
        {
            outputText.text = "Definitely";

        }
        else
        {
            outputText.text = "I don't know";
        }
    }
}
