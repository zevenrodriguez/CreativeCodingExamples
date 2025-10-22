using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageGallery : MonoBehaviour
{
    [SerializeField] Image chameleon;
    [SerializeField] Image chimpanzee;
    [SerializeField] Image elephant;
    [SerializeField] Image hedgehog;

    [SerializeField] int imagePicker = 0;

    [SerializeField] Button next;
    [SerializeField] Button prev;

    [SerializeField] Button startButton;
    [SerializeField] Button stopButton;


    // Start is called before the first frame update
    void Start()
    {
    //    chameleon.enabled = true;
    //    chimpanzee.enabled = false;
    //    elephant.enabled = false;
    //    hedgehog.enabled = false;
        //ChangeImage(5);
        ChangeImage(imagePicker); 
        next.onClick.AddListener(()=>{
            imagePicker = imagePicker + 1;
            ChangeImage(imagePicker);
            if(imagePicker >= 4){
                imagePicker = 0;
                ChangeImage(imagePicker);
            }
        });

        prev.onClick.AddListener(()=>{
            imagePicker = imagePicker - 1;
            ChangeImage(imagePicker);
            if(imagePicker <= -1){
                imagePicker = 3;
                ChangeImage(imagePicker);
            }
        });

        startButton.onClick.AddListener(()=>{
            StartCoroutine("NextImage", 5.0f);
        });

        stopButton.onClick.AddListener(()=>{
            StopCoroutine("NextImage");
        });

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ChangeImage(int curImage){
        if(curImage == 0){
            chameleon.enabled = true;
            chimpanzee.enabled = false;
            elephant.enabled = false;
            hedgehog.enabled = false;
        }else if(curImage == 1){
            chameleon.enabled = false;
            chimpanzee.enabled = true;
            elephant.enabled = false;
            hedgehog.enabled = false;
        }else if(curImage == 2){
            chameleon.enabled = false;
            chimpanzee.enabled = false;
            elephant.enabled = true;
            hedgehog.enabled = false;
        }else if(curImage == 3){
            chameleon.enabled = false;
            chimpanzee.enabled = false;
            elephant.enabled = false;
            hedgehog.enabled = true;
        }else{
            //default statement
            chameleon.enabled = false;
            chimpanzee.enabled = false;
            elephant.enabled = false;
            hedgehog.enabled = false;
            
        }
    }

    IEnumerator NextImage(float interval){
        Debug.Log("next image");
        imagePicker = imagePicker + 1;
            ChangeImage(imagePicker);
            if(imagePicker >= 4){
                imagePicker = 0;
                ChangeImage(imagePicker);
            }
        //yield return null; null waits for the next/current frame to finish
        yield return new WaitForSeconds(interval);
        Debug.Log("yield done");
        StartCoroutine("NextImage", 5.0f);
    }
}
