using UnityEngine;
using TMPro;
public class ScoreUpdate : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    int currentScore = 3;

    void Start()
    {
        updateText();
    }

    public void increaseScore(int changeScore)
    {
        currentScore = currentScore + changeScore;
    }

    public void decreaseScore(int changeScore)
    {
        currentScore = currentScore - changeScore;
    }

    public void updateText()
    {
        if (currentScore <= 0)
        {
            scoreText.text = "Game Over";
        }
        else
        {
            scoreText.text = "Your Score: " + currentScore.ToString();
        }
        
    }
}
