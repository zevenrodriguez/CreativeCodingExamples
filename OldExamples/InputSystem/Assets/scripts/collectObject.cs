using UnityEngine;

public class collectObject : MonoBehaviour
{
    [SerializeField] UpdateScore scoreManager;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        this.gameObject.SetActive(false);
        scoreManager.currentScore += 1;
        scoreManager.UpdateText();
    }

}
