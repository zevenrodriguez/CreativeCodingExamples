using UnityEngine;
using TMPro;

public class UpdateScore : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public int currentScore = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        scoreText.text = "Score: " + currentScore.ToString();
    }
}
