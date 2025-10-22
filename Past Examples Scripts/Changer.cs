using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Changer : MonoBehaviour
{
    // Start is called before the first frame update

    //[SerializeField] Material changer;
    Material changer;
    [SerializeField] GameObject sphere;
    [SerializeField] Slider red;
    [SerializeField] Slider green;
    [SerializeField] Slider blue;
    [SerializeField] TMP_Dropdown colorPicker;

    void Start()
    {
      changer = sphere.GetComponent<Renderer>().material;

      red.onValueChanged.AddListener(delegate{
        Debug.Log(red.value);
        changer.color = new Color(red.value,changer.color.g,changer.color.b);
      }); 
      green.onValueChanged.AddListener(delegate{
        Debug.Log(green.value);
        changer.color = new Color(changer.color.r,green.value,changer.color.b);
      });
      blue.onValueChanged.AddListener(delegate{
        Debug.Log(green.value);
        changer.color = new Color(changer.color.r,changer.color.g,blue.value);
      }); 

      //dropdown
      colorPicker.onValueChanged.AddListener(delegate{
        Debug.Log(colorPicker.value);
        string currentText = colorPicker.options[colorPicker.value].text;
        Debug.Log(currentText);
        //in an or statement either side of the || (pipe) has to be true
        if(colorPicker.value == 0 || currentText == "White"){
          changer.color = Color.white;
        }else if(colorPicker.value == 1 || currentText == "Red"){
          changer.color = Color.red;
        }else if(colorPicker.value == 2 || currentText == "Green"){
          changer.color = Color.green;
        }else if(colorPicker.value == 3 || currentText == "Blue"){
          changer.color = Color.blue;
        }
      });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
