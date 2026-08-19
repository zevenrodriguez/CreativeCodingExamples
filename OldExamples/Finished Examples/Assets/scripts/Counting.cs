using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Counting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int number = 10; //whole number
    float decimals = 25.0f;
    bool answer = false;
    char letter = 'a';
    string word = "hello";

    [SerializeField] int interval = 10;
    [SerializeField] float intervalFloat = 0.00001f;
    float numberFloat = 0.0f;
    [SerializeField] TMP_Text output;

    [SerializeField] Image imageContainer;
    [SerializeField] Sprite sprite0;
    [SerializeField] Sprite sprite1;
    [SerializeField] Sprite sprite2;
    [SerializeField] Sprite sprite3;
    [SerializeField] Sprite sprite4;
    void Start()
    {
        Debug.Log("Hello World");
        Debug.Log(number);

        imageContainer.sprite = sprite0;
        imageContainer.SetNativeSize();
    }

    // Update is called once per frame
    void Update()
    {
        //number = number + 1;
        //number = number + interval;
        numberFloat = numberFloat + intervalFloat;
        number = (int)numberFloat;
        Debug.Log("number: " + number + " numberFloat: " + numberFloat);
        output.text = number.ToString();

        if (number > 20 && number <= 40)
        {
            imageContainer.sprite = sprite1;
            imageContainer.SetNativeSize();
        }
        else if (number > 40 && number <= 60)
        {
            imageContainer.sprite = sprite2;
            imageContainer.SetNativeSize();
        }
        else if (number > 60 && number <= 80)
        {
            imageContainer.sprite = sprite3;
            imageContainer.SetNativeSize();
        }
        else if (number > 80 && number <= 100)
        {
            imageContainer.sprite = sprite4;
            imageContainer.SetNativeSize();
        }
        else if (number > 100)
        {

            number = 0;
            numberFloat = 0.0f;
        }
        else
        {
            imageContainer.sprite = sprite0;
        }
    }
}
