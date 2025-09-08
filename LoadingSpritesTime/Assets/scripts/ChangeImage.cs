using UnityEngine;
using UnityEngine.UI;
public class ChangeImage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Image mainImage;
    [SerializeField] Sprite image0;
    [SerializeField] Sprite image1;
    [SerializeField] Sprite image2;
    [SerializeField] Sprite image3;
    [SerializeField] Sprite image4;

    float speed = 0.01f;
    float currentCount = 0.0f;
    void Start()
    {
        mainImage.sprite = image0;
        mainImage.SetNativeSize();
    }

    // Update is called once per frame
    void Update()
    {
        currentCount = currentCount + speed;
        Debug.Log(currentCount);
        if (currentCount > 0 && currentCount < 60)
        {
            mainImage.sprite = image0;
            mainImage.SetNativeSize();
        }
        else if (currentCount > 60 && currentCount < 120)
        {
            mainImage.sprite = image1;
            mainImage.SetNativeSize();
        }
        else if (currentCount > 120 && currentCount < 180)
        {
            mainImage.sprite = image2;
            mainImage.SetNativeSize();
        }
        else if (currentCount > 180 && currentCount < 240)
        {
            mainImage.sprite = image3;
            mainImage.SetNativeSize();
        }
        else if (currentCount > 240 && currentCount < 300)
        {
            mainImage.sprite = image4;
            mainImage.SetNativeSize();
        }
        else if(currentCount > 300)
        {
            currentCount = 0;
        }
    }
}
