using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AnimalMashUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Button dragon;
    [SerializeField] Button eagle;
    [SerializeField] Button turtle;
    [SerializeField] Button shark;
    [SerializeField] Button panda;
    [SerializeField] Button lion;

    [SerializeField] Button generateButton;
    [SerializeField] Image mashUpImage;
    string animal1 = "";
    string animal2 = "";

    [SerializeField] Sprite dragonLionSprite;
    [SerializeField] Sprite eaglePandaSprite;
    [SerializeField] Sprite turtleSharkSprite;
 


    void Start()
    {
       dragon.onClick.AddListener(() => { animal1 = "dragon"; });
       eagle.onClick.AddListener(() => { animal1 = "eagle"; });
       turtle.onClick.AddListener(() => { animal1 = "turtle"; });
       shark.onClick.AddListener(() => { animal2 = "shark"; });
       panda.onClick.AddListener(() => { animal2 = "panda"; });
       lion.onClick.AddListener(() => { animal2 = "lion"; });
      
       generateButton.onClick.AddListener(() => { 
        mashUpImage.sprite = GenerateMashUp(animal1, animal2);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Sprite GenerateMashUp(string animal1, string animal2)
    {
        if (animal1 == "dragon" && animal2 == "lion")
        {
            return dragonLionSprite;
        }
        else if (animal1 == "eagle" && animal2 == "panda")
        {
            return eaglePandaSprite;
        }
        else if (animal1 == "turtle" && animal2 == "shark")
        {
            return turtleSharkSprite;
        }
        else
        {
          return null;  
        }
    }
}
