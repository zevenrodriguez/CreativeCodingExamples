using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trivia : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TMP_Text questionOutput;
    [SerializeField] TMP_InputField answerInput;
    [SerializeField] Button submitButton;

    //string currentQuestion = "Which animal can change color";
    string currentAnswer = "chameleon";
    int currentQuestionIndex = 0;
    int score = 0;
 
    void Start()
    {
        submitButton.onClick.AddListener(SubmitAnswer);
        questionOutput.text = "Which animal can change color?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SubmitAnswer()
    {
        string answer = answerInput.text;
        Debug.Log("Submitted Answer: " + answer);
        // Here you can add logic to check the answer and provide feedback
        
        if (answer.ToLower() == currentAnswer)
        {
            //score = score + 1;
            AddScore();
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Incorrect. The correct answer is: " + currentAnswer + ".");
        }
        currentQuestionIndex = currentQuestionIndex + 1;
        nextQuestion(currentQuestionIndex);

        // if (currentQuestionIndex == 1)
        // {
        //     questionOutput.text = "Which animal has a trunk?";
        //     currentAnswer = "elephant";
        // }
        // else if (currentQuestionIndex == 2)
        // {
        //     questionOutput.text = "A group of this animal is called an array?";
        //     currentAnswer = "hedgehog";
        // }
        // else if (currentQuestionIndex == 3)
        // {
        //     questionOutput.text = "Which of these animals is our closest relative?";
        //     currentAnswer = "chimpanzee";
        // }
        // else
        // {
        //     questionOutput.text = "Trivia completed! your score is: " + score;
        //     submitButton.enabled = false;
        // } 


    }

    void AddScore()
    {
        score = score + 1;
    }

    void nextQuestion(int questionIndex)
    {

        if (questionIndex == 1)
        {
            questionOutput.text = "Which animal has a trunk?";
            currentAnswer = "elephant";
        }
        else if (questionIndex == 2)
        {
            questionOutput.text = "A group of this animal is called an array?";
            currentAnswer = "hedgehog";
        }
        else if (questionIndex == 3)
        {
            questionOutput.text = "Which of these animals is our closest relative?";
            currentAnswer = "chimpanzee";
        }
        else
        {
            questionOutput.text = "Trivia completed! your score is: " + score;
            submitButton.enabled = false;
        } 
    }

}
