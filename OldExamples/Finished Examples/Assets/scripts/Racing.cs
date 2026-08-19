using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Racing : MonoBehaviour
{
    [SerializeField] Transform player1;

    [SerializeField] float speedXSphere = 0.01f;

    [SerializeField] Button startButton;

    bool raceStart = false;

    [SerializeField] TMP_Text output;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(startRace);
    }

    // Update is called once per frame
    void Update()
    {
        if (raceStart == true)
        {
            float spherePositionX = player1.position.x + Random.Range(-0.01f,0.02f);
            player1.position = new Vector3(spherePositionX, player1.position.y, player1.position.z);

        }

        if (player1.position.x > 9)
        {
            Debug.Log("player 1 wins");
            raceStart = false;
            output.text = "Player 1 Wins";
        }

    }

    void startRace()
    {
        Debug.Log("Start the race!");
        raceStart = true;
    }
}
